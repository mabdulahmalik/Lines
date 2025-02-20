<script lang="ts" setup>
import { ref, computed } from 'vue';
import { FwbInput } from 'flowbite-vue';
import { IconSearchOutline } from '@/components/icons/index';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';

const props = defineProps<{
  purposes?: Array<{ id: string; name: string; count: number }>;
  modelValue: Array<{ id: string; name: string; count: number }>;
}>();

const emit = defineEmits(['update:modelValue']);

const selectedPurposes = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit('update:modelValue', val);
  },
});

function isSelected(purposeId: string): boolean {
  return selectedPurposes.value.some((item) => item.id === purposeId);
}

// Updates selectedPurposes based on checkbox interactions
function updateMultiSelectValue(
  purpose: { id: string; name: string; count: number },
  isSelected: boolean
) {
  if (isSelected) {
    selectedPurposes.value.push(purpose);
  } else {
    selectedPurposes.value = selectedPurposes.value.filter((item) => item.id !== purpose.id);
  }
}

// search query
const searchQuery = ref('');
const filteredPurposesList = computed(() => {
  return (
    props.purposes?.filter((purpose) =>
      purpose.name.toLowerCase().includes(searchQuery.value.toLowerCase())
    ) || []
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
        v-for="purpose in filteredPurposesList"
        :key="purpose.id"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(purpose.id) ? purpose.id : '']"
          @update:modelValue="
            (newValue) => updateMultiSelectValue(purpose, newValue.includes(purpose.id))
          "
          :label="purpose.name"
          :value="purpose.id"
          class="gap-0.5"
        />
        <!-- <fwb-badge type="dark">{{ purpose.count }}</fwb-badge> -->
      </div>
    </div>
  </div>
</template>
