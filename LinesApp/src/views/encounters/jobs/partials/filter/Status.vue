<script lang="ts" setup>
import { ref, computed } from 'vue';
import { FwbInput } from 'flowbite-vue';
import { IconSearchOutline } from '@/components/icons/index';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';

const props = defineProps<{
  status?: Array<{ name: string; count: number }>;
  modelValue: Array<{ name: string; count: number }>;
}>();
const emit = defineEmits(['update:modelValue']);

const selectedStatuses = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit('update:modelValue', val);
  },
});

function isSelected(statusName: string): boolean {
  return selectedStatuses.value.some((item) => item.name === statusName);
}

// Updates selectedStatuses based on checkbox interactions
function updateMultiSelectValue(statusName: string, count: number, isSelected: boolean) {
  if (isSelected) {
    selectedStatuses.value.push({ name: statusName, count });
  } else {
    selectedStatuses.value = selectedStatuses.value.filter((item) => item.name !== statusName);
  }
}

// search query
const searchQuery = ref('');
const filteredStatusList = computed(() => {
  return (
    props.status?.filter((status) =>
      status.name.toLowerCase().includes(searchQuery.value.toLowerCase())
    ) || []
  );
});

const capitalize = (text: string): string => {
  return text.toLowerCase().replace(/\b\w/g, (char) => char.toUpperCase());
};
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
        v-for="(status, indx) in filteredStatusList"
        :key="indx"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(status.name) ? status.name : '']"
          @update:modelValue="
            (newValue) =>
              updateMultiSelectValue(status.name, status.count, newValue.includes(status.name))
          "
          :label="capitalize(status.name)"
          :value="status.name"
          class="gap-0.5"
        />
        <!-- <fwb-badge type="dark">{{ status.count }}</fwb-badge> -->
      </div>
    </div>
  </div>
</template>
