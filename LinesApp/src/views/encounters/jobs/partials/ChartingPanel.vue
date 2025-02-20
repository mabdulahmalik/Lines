<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import { IconXClose } from '@/components/icons/index';

const props = withDefaults(
  defineProps<{
    title?: string;
    defaultOpen?: boolean;
  }>(),
  {
    defaultOpen: false,
  }
);
const emit = defineEmits<{
  (e: 'close'): void;
}>();
const panelOpen = ref(props.defaultOpen);

const setModalOpen = (value: boolean): void => {
  if (!value) {
    emit('close');
  }
  panelOpen.value = value;
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <transition name="slide" appear>
    <div
      v-if="panelOpen"
      class="sm:max-w-[470px] w-full duration-300 ease-linear bg-white border-l border-l-slate-200 lg:relative fixed right-0 lg:top-0 top-[119px] min-h-[calc(100vh-192px)] lg:min-h-[calc(100vh-152px)] z-[9]"
    >
      <!-- Header -->
      <div class="lg:hidden flex border-slate-300 items-center border-y border-l-slate-200">
        <div v-if="props.title" class="flex items-center gap-4 p-4">
          <fwb-button
            v-if="props.title"
            @click="setModalOpen(false)"
            color="light"
            pill
            square
            size="sm"
            class="bg-white border-white text-slate-400 hover:text-slate-900"
          >
            <IconXClose width="20" height="20" />
          </fwb-button>
          <span class="text-lg font-semibold text-slate-900">{{ props.title }}</span>
        </div>
      </div>
      <!-- body -->
      <div
        class="lg:overscroll-y-none overflow-y-auto custom-scroll overflow-x-clip lg:h-auto sm:h-[calc(100vh-245px)] h-[calc(100vh-312px)]"
      >
        <slot name="body">Body</slot>
      </div>
    </div>
  </transition>
</template>

<style scoped>
/* Transition for the panel */
.slide-enter-active,
.slide-leave-active {
  transition: transform 0.7s ease-out;
}
.slide-enter-from,
.slide-leave-to {
  transform: translateX(100%);
}
</style>
