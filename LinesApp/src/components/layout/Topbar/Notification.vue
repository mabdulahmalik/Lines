<script setup lang="ts">
import { IconXClose } from "@/components/icons/index";
interface Notification {
  date?: string;
  time?: string;
  job_name?: string;
  created_by: string;
  location: string;
}

defineProps<{
  notification: Notification;
  index: number
}>();

const emit = defineEmits<{
  (e: 'remove', index: number): void;
}>();

const removeNotification = (index: number) => {
  emit('remove', index);
};
</script>
<template>
  <div
    class="group m-1 flex items-center justify-between bg-white hover:bg-brand-50 bg-white hover:bg-brand-50 px-4 py-2 rounded-lg duration-200 ease-out"
  >
    <div class="flex flex-col text-slate-500 font-medium">
      <div class="text-xs">
        <span> {{ notification.date }}</span>
        <span> {{ notification.time }} </span>
      </div>
      <div class="text-sm">
        <span class="text-slate-900"> {{ notification.job_name }} </span>
        created by
        <span class="text-slate-900">{{ notification.created_by }}</span> in
        <span class="text-slate-900">{{ notification.location }} </span>
      </div>
    </div>
    <span @click="removeNotification(index)" class="text-brand-600 cursor-pointer opacity-0 group-hover:opacity-100">
      <IconXClose width="16" height="17" :stroke-width="2" />
    </span>
  </div>
  <hr class="border-slate-200 height-1">
</template>
