<script setup lang="ts">
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { computed } from 'vue';

const props = defineProps<{
  procedure: any
}>();

const proceduresStore = useProceduresStore();

const procedure = computed(() => proceduresStore.procedures.find(x => x.id === props.procedure.ProcedureId))

// Helper function to find the field by its ID
const getFieldById = (fieldId: string) => {
  return procedure.value?.fields?.find((field: any) => field.id === fieldId);
}

</script>

<template>
  <div class="font-medium text-sm p-4 rounded-lg bg-slate-50">
    <div class="grid grid-cols-2 gap-4 text-justify">
      <span class="text-slate-500">Procedure Name</span>      
      <span class="text-slate-800 md:text-left text-right">{{ procedure?.name }}</span>
    </div>
    <div class="grid md:grid-cols-2 gap-4 mt-4">
    <!-- Loop through procedure values and render fields dynamically -->
    <div v-for="value in props.procedure.Values" :key="value.FieldId" class="grid grid-cols-2 gap-2">
      <span class="text-slate-500">{{ getFieldById(value.FieldId)?.name }}</span>
      <span class="text-slate-800 ">
        <!-- Render field value based on field type -->
        <template v-if="getFieldById(value.FieldId)?.type === 'TEXT'">
          {{ value.Value }}
        </template>
        <template v-if="getFieldById(value.FieldId)?.type === 'TOGGLE'">
          {{ value.Value === 'on' ? 'Yes' : 'No' }}
        </template>
        <template v-if="getFieldById(value.FieldId)?.type === 'LIST'">
          <!-- Find the option text for list type fields -->
          {{
            getFieldById(value.FieldId)?.options?.find(option => option?.id === value.Value)?.text
          }}
        </template>
        <template v-if="getFieldById(value.FieldId)?.type === 'NUMBER'">
          {{ value.Value }}
        </template>
      </span>
    </div>
  </div>
  </div>
</template>
