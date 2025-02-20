<script setup lang="ts">
import { FwbButton, FwbTooltip } from 'flowbite-vue';
import { IconEdit03, IconLinkExternal02 } from '@/components/icons/index';
const props = withDefaults(
  defineProps<{
    title: string;
    isEdit?: boolean;
    caption?: string;
    isLink?: boolean;
  }>(),
  {
    isEdit: false,
    isLink: false,
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
  <div class="w-full flex items-center justify-between">
    <span class="text-base font-semibold"> {{ props.title }} </span>
    <div class="text-slate-500 hover:text-slate-900">
      <fwb-tooltip v-if="!isEdit" placement="bottom">
        <template #trigger>
          <fwb-button @click.stop="edit" color="light" pill square class="border-transparent">
            <IconEdit03 v-if="!isLink"/>
            <IconLinkExternal02 v-else />
          </fwb-button>
        </template>
        <template #content> {{props.caption ?? 'Bulk edit'}} </template>
      </fwb-tooltip>
    </div>
  </div>
</template>
