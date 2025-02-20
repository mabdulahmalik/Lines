<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';

const props = defineProps<{
  title?: string;
  description?: string;
  buttonText?: string;
}>();

const emit = defineEmits<{
  (e: 'action'): void;
}>();
</script>

<template>
  <div class="flex justify-center text-center sm:p-7 p-4">
    <div class="max-w-lg flex flex-col justify-center items-center relative">
      <div class="z-[2]">
        <slot name="image"></slot>
      </div>
      <div
        class="rounded-full w-[450px] h-[450px] flex justify-center items-center overflow-hidden absolute sm:top-[-97px] top-[-85px]"
      >
        <img src="@/assets/images/encounters/background-dots-grid.svg" alt="dots-grid" />
      </div>
      <div class="text-2xl leading-9 font-semibold text-slate-900 mt-3" v-html="props.title"></div>
      <p class="text-slate-500 font-medium text-base mb-8 mt-2" v-html="props.description"></p>
      <fwb-button
        v-if="buttonText"
        size="lg"
        class="w-60 bg-brand-600 hover:bg-brand-600 z-[4]"
        pill
        @click.prevent="$emit('action')"
      >
        {{ props.buttonText }}
      </fwb-button>
    </div>
  </div>
</template>
