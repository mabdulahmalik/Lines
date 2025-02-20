<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import { onClickOutside } from '@vueuse/core';

const props = withDefaults(
  defineProps<{
    title?: string;
    size?: string;
    full_height?: boolean,
    z_index?: number;
    set_margins?: boolean;
    no_footer?: boolean;
    no_header?: boolean;
    close_outside?: boolean;
  }>(),
  {
    size: 'sm',
    full_height: false,
    z_index: 49,
    set_margins: false,
    no_footer: false,
    no_header: false,
    close_outside: true,
  }
);

const emit = defineEmits<{
  (e: 'close'): void;
}>();

const modalOpen = ref(false);

const modalContainerRef = ref<HTMLElement | null>(null);

const setModalOpen = (value: boolean): void => {
  if(!value){
    emit('close');
  }
  modalOpen.value = value;
};

onClickOutside(modalContainerRef, () => {
  if (props.close_outside) {
    setModalOpen(false);
  }
});

defineExpose({
  setModalOpen,
});
</script>

<template>
  <!-- Main modal -->
  <div tabindex="-1" v-bind="modalOpen ? { 'aria-modal': 'true' } : { 'aria-hidden': 'true' }">
    <transition name="fade" appear>
      <div
        v-if="modalOpen"
        class="fixed inset-0 bg-black bg-opacity-70 z-[49]"
        :style="{ zIndex: z_index }"
        @click="setModalOpen(false)"
      ></div>
    </transition>
    <transition name="fade" appear>
      <div
        v-if="modalOpen"
        class="z-50 p-4 lg:p-6 flex overflow-x-hidden fixed top-0 right-0 left-0 justify-center items-center w-auto md:inset-0 h-full max-h-full"
        :style="{ zIndex: z_index + 1 }"
      >
        <div class="relative w-full max-h-full" :class="[full_height ? 'h-full' : 'h-min-content', `max-w-${size}`]">
          <!-- Modal Container -->
          <div
            ref="modalContainerRef"
            class="relative h-full bg-white shadow-2xl rounded-xl border border-slate-200 dark:bg-gray-700 flex flex-col"
          >
            <!-- Modal header -->
            <div
              v-if="!props.no_header"
              class="flex border-b border-slate-300 items-center justify-between"
              :class="{ 'py-4 px-4 lg:px-6': props.set_margins }"
            >
              <div class="w-full text-lg font-semibold text-brand-900">
                <div v-if="title" :class="{ 'px-4 py-4 lg:px-6': !props.set_margins }">
                  {{ title }}
                </div>
                <slot v-else name="header"></slot>
              </div>
              <!-- Close button -->
              <fwb-button
                @click="setModalOpen(false)"
                color="light"
                pill
                square
                size="lg"
                class="bg-white border-white text-slate-400 hover:text-slate-900"
                :class="
                  [
                    !props.set_margins && props.title ? 'mr-4 lg:mr-6' : '',
                  ].join('')
                "
              >
                <svg
                  class="w-3 h-3"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 14 14"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                  />
                </svg>
              </fwb-button>
            </div>
            <!-- Modal body -->
            <div class="flex-1 overflow-y-auto" :class="{ 'py-4 px-4 lg:px-6': props.set_margins }">
              <slot name="body">Body</slot>
            </div>
            <!-- Modal footer -->
            <div
              v-if="!props.no_footer"
              class="flex items-center border-t border-state-300"
              :class="{ 'py-4 px-4 lg:px-6': props.set_margins }"
            >
              <slot name="footer">
                <div :class="{ 'py-4 px-4 lg:px-6': !props.set_margins }">
                  <fwb-button @click="setModalOpen(false)" color="light" pill> Close</fwb-button>
                </div>
              </slot>
            </div>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<style scoped>
/* Transition for the backdrop */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
