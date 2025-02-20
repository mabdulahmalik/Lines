<script lang="ts" setup>
import { ref, computed } from 'vue';
import { FwbInput } from 'flowbite-vue';
import { IconSearchOutline } from '@/components/icons/index';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';

const props = defineProps<{
  assigned?: Array<{ id: string; name: string; count: number }>;
  modelValue: Array<{ id: string; name: string; count: number }>;
}>();

const emit = defineEmits(['update:modelValue']);

const selectedAssigned = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit('update:modelValue', val);
  },
});

function isSelected(assignedId: string): boolean {
  return selectedAssigned.value.some((item) => item.id === assignedId);
}

// Updates selectedAssigned based on checkbox interactions
function updateMultiSelectValue(
  assignedId: string,
  assignedName: string,
  count: number,
  isSelected: boolean
) {
  if (isSelected) {
    selectedAssigned.value.push({ id: assignedId, name: assignedName, count });
  } else {
    selectedAssigned.value = selectedAssigned.value.filter((item) => item.id !== assignedId);
  }
}

// search query
const searchQuery = ref('');
const filteredAssignedList = computed(() => {
  return (
    props.assigned
      ?.filter((assign) => assign.name.toLowerCase().includes(searchQuery.value.toLowerCase()))
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
        v-for="assigned in filteredAssignedList"
        :key="assigned.id"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(assigned.id) ? assigned.id : '']"
          @update:modelValue="
            (newValue) =>
              updateMultiSelectValue(
                assigned.id,
                assigned.name,
                assigned.count,
                newValue.includes(assigned.id)
              )
          "
          :label="assigned.name"
          :value="assigned.id"
          class="gap-0.5"
        />
        <!-- fwb-badge type="dark">{{ assigned.count }}</fwb-badge -->
      </div>
    </div>
  </div>
</template>
