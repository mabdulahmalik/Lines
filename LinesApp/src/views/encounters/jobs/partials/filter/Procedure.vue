<script lang="ts" setup>
import { ref, computed } from 'vue';
import { FwbInput, FwbBadge } from 'flowbite-vue';
import { IconSearchOutline } from '@/components/icons/index';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';

const props = defineProps<{
  procedures?: Array<{ name: string; count: number }>;
  modelValue: Array<{ name: string; count: number }>;
}>();

const emit = defineEmits(['update:modelValue']);

const selectedProcedures = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit('update:modelValue', val);
  },
});

function isSelected(procedureName: string): boolean {
  return selectedProcedures.value.some((item) => item.name === procedureName);
}

// Updates selectedProcedures  based on checkbox interactions
function updateMultiSelectValue(procedureName: string, count: number, isSelected: boolean) {
  if (isSelected) {
    selectedProcedures.value.push({ name: procedureName, count });
  } else {
    selectedProcedures.value = selectedProcedures.value.filter(
      (item) => item.name !== procedureName
    );
  }
}

// search query
const searchQuery = ref('');
const filteredProceduresList = computed(() => {
  return (
    props.procedures?.filter((status) =>
      status.name.toLowerCase().includes(searchQuery.value.toLowerCase())
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
        v-for="(procedure, indx) in filteredProceduresList"
        :key="indx"
        class="flex items-center justify-between"
      >
        <CustomCheckbox
          :modelValue="[isSelected(procedure.name) ? procedure.name : '']"
          @update:modelValue="
            (newValue) =>
              updateMultiSelectValue(
                procedure.name,
                procedure.count,
                newValue.includes(procedure.name)
              )
          "
          :label="procedure.name"
          :value="procedure.name"
          class="gap-0.5"
        />
        <fwb-badge type="dark">{{ procedure.count }}</fwb-badge>
      </div>
    </div>
  </div>
</template>
