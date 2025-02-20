<script setup lang="ts">
import { FwbButton, FwbTooltip } from 'flowbite-vue';
import { IconEdit03 } from '@/components/icons/index';
const props = withDefaults(
  defineProps<{
    title: string;
    isEdit?: boolean;
    archived?: boolean;
  }>(),
  {
    isEdit: false,
    archived:false,
  }
);
const emit = defineEmits<{
  (e: 'edit'): void;
}>();

const edit = () => {
  emit('edit');
};
</script>
<template>
  <div class="w-full flex items-center justify-between ">
    <span class="text-base font-semibold"> {{ props.title }} </span>
     <div v-if="!archived" class="text-slate-500 hover:text-slate-900">
      <fwb-tooltip v-if="!isEdit" placement="bottom" >
        <template #trigger>
          <fwb-button @click.stop="edit" color="light" pill square class="border-transparent p-0">
            <IconEdit03/>
          </fwb-button>
        </template>
        <template #content> Bulk edit </template>
      </fwb-tooltip>
    </div> 
  </div>
</template>
