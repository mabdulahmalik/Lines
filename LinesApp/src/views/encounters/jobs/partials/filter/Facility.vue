<script lang="ts" setup>
import { ref, computed } from 'vue';
import { FwbInput} from 'flowbite-vue';
import { IconSearchOutline } from '@/components/icons/index';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';

const props = defineProps<{
  facility?: Array<{ id: string; name: string; count: number }>;
  modelValue: Array<{ id: string; name: string; count: number }>;
}>();

const emit = defineEmits(['update:modelValue']);

const selectedFacility = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit('update:modelValue', val);
  },
});

// Check if a facility is selected by id
function isSelected(facilityId: string): boolean {
  return selectedFacility.value.some((item) => item.id === facilityId);
}

// Updates selectedFacility based on checkbox interactions
function updateMultiSelectValue(
  facilityId: string,
  facilityName: string,
  count: number,
  isSelected: boolean
) {
  if (isSelected) {
    selectedFacility.value.push({ id: facilityId, name: facilityName, count });
  } else {
    selectedFacility.value = selectedFacility.value.filter((item) => item.id !== facilityId);
  }
}

// search query
const searchQuery = ref('');
const filteredFacilityList = computed(() => {
  return (
    props.facility
      ?.filter((facility) => facility.name.toLowerCase().includes(searchQuery.value.toLowerCase()))
      .sort((a, b) => a.name.localeCompare(b.name)) || []
  );
});
</script>

<template>
  <div class="flex flex-col gap-4 lg:gap-6">
    <fwb-input v-model.trim="searchQuery" placeholder="Search">
      <template #prefix>
        <IconSearchOutline width="20" height="20" class="text-gray-500 dark:text-gray-400" />
      </template>
    </fwb-input>
    <div class="flex flex-col gap-4">
      <div
        v-for="facility in filteredFacilityList"
        :key="facility.id"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(facility.id) ? facility.id : '']"
          @update:modelValue="
            (newValue) =>
              updateMultiSelectValue(
                facility.id,
                facility.name,
                facility.count,
                newValue.includes(facility.id)
              )
          "
          :label="facility.name"
          :value="facility.id"
          class="gap-0.5"
        />
        <!-- <fwb-badge type="dark">{{ facility.count }}</fwb-badge> -->
      </div>
    </div>
  </div>
</template>
