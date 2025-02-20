<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import { onClickOutside } from '@vueuse/core';

const props = withDefaults(
  defineProps<{
    title?: string;
    max_width?: string;
    auto_height?: boolean;
    left_close_icon?: boolean;
    hide_header_close_on_mobile?: boolean;
    z_index?: number;
    set_margins?: boolean;
    no_footer?: boolean;
    no_header?: boolean;
    close_outside?: boolean;
    sm_margin?: boolean;
  }>(),
  {
    max_width: '5xl',
    auto_height: false,
    hide_header_close_on_mobile: false,
    z_index: 51,
    set_margins: false,
    no_footer: false,
    no_header: false,
    close_outside: false,
    sm_margin: false,
    left_close_icon: false,
  }
);
const emit = defineEmits<{
  (e: 'close'): void;
}>();

const modalOpen = ref(false);

const modalContainer = ref<HTMLElement | null>(null);

const setModalOpen = (value: boolean): void => {
  if (!value) {
    emit('close');
  }
  modalOpen.value = value;
};

onClickOutside(modalContainer, () => {
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
        class="fixed inset-0 bg-slate-50 z-[51]"
        :style="{ zIndex: z_index }"
        @click="setModalOpen(false)"
      ></div>
    </transition>
    <transition name="fade" appear>
      <div
        v-if="modalOpen"
        v-no-scroll="modalOpen"
        class="z-[52] flex flex-col overflow-x-hidden fixed top-0 right-0 left-0 justify-start items-center w-auto md:inset-0 h-full max-h-full"
        :style="{ zIndex: z_index + 1 }"
      >
        <div
          class="flex items-center justify-center w-full p-4 h-16 lg:h-20 bg-header-gradient z-[51]"
        >
          <a href="#">
            <img src="/lines-logo-white.svg" class="h-8 sm:h-11" alt="Lines Logo" />
          </a>
        </div>
        <div
          class="relative flex items-center w-full h-[calc(100%-64px)] lg:h-[calc(100%-80px)] max-h-full duration-300 ease-linear"
          :class="`max-w-${max_width}`"
        >
          <!-- Modal Container -->
          <div
            ref="modalContainer"
            class="relative w-full bg-white shadow-xl border border-[#CBD5E1] flex flex-col"
            :class="[auto_height ? 'h-auto' : 'h-full', sm_margin ? 'm-4 lg:m-0' : '']"
          >
            <!-- Modal header -->
            <div
              v-if="!props.no_header"
              class="flex border-b border-slate-300 items-center"
              :class="{ 'py-4 px-4 lg:px-6': props.set_margins }"
            >
              <div class="w-full">
                <div
                  v-if="props.title"
                  class="flex items-center"
                  :class="{ 'px-4 py-4 lg:px-6': !props.set_margins }"
                >
                  <!-- Close button -->
                  <fwb-button
                    v-if="props.left_close_icon"
                    @click="setModalOpen(false)"
                    color="light"
                    pill
                    square
                    size="lg"
                    class="mr-2 bg-white border-white text-slate-400 hover:text-slate-900"
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
                  <span class="text-lg font-semibold text-slate-900">{{ props.title }}</span>
                </div>
                <slot v-else name="header"></slot>
              </div>
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

/* Transition for the modal drawer */
.slide-enter-active,
.slide-leave-active {
  transition: transform 0.7s ease-out;
}
.slide-enter-from,
.slide-leave-to {
  transform: translateX(100%);
}
</style>
