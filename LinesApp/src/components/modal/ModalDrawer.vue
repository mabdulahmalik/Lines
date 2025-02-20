<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import { onClickOutside } from '@vueuse/core';
import { IconArrowLeft } from '@/components/icons/index';

const props = withDefaults(
  defineProps<{
    title?: string;
    max_width?: string;
    hide_header_close_on_mobile?: boolean;
    left_close_inside?: boolean;
    z_index?: number;
    set_margins?: boolean;
    no_footer?: boolean;
    no_header?: boolean;
    close_outside?: boolean;
  }>(),
  {
    max_width: '5xl',
    hide_header_close_on_mobile: false,
    left_close_inside: false,
    z_index: 49,
    set_margins: false,
    no_footer: false,
    no_header: false,
    close_outside: false,
  }
);
const emit = defineEmits<{
  (e: 'close'): void;
}>();
const modalOpen = ref(false);

const modalContainer = ref<HTMLElement | null>(null);

const setModalOpen = (value: boolean): void => {
  if(!value){
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
  <div tabindex="-1" v-bind="modalOpen ? {'aria-modal': 'true'} : { 'aria-hidden': 'true' }">
    <transition name="fade" appear>
      <div
        v-if="modalOpen"
        class="fixed inset-0 bg-black bg-opacity-70 z-[49]"
        :style="{ zIndex: z_index }"
        @click="setModalOpen(false)"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div
        v-if="modalOpen"
        v-no-scroll="modalOpen"
        class="z-50 flex overflow-x-hidden fixed top-0 right-0 left-0 justify-end items-end w-auto md:inset-0 h-full max-h-full"
        :class="{ 'lg:pl-4': max_width !== 'full' }"
        :style="{ zIndex: z_index + 1 }"
      >
        <div
          class="relative w-full h-[calc(100%-64px)] lg:h-[calc(100%-80px)] max-h-full duration-300 ease-linear"
          :class="`max-w-${max_width}`"
        >
          
          <!-- Modal Container -->
          <div
            ref="modalContainer"
            class="relative h-full bg-white shadow-2xl dark:bg-gray-700 flex"
          >
            <slot name="sidebar">
            </slot>
            <div class="flex flex-1 flex-col h-full w-[calc(100%-64px)]">

              <!-- OnHold Slot -->
              <slot name="onHold"></slot>

              <!-- Assistance Slot -->
              <slot name="assistance"></slot>

              <!-- Reschedule Slot -->
              <slot name="reschedule"></slot>
              <!-- Modal header -->
              <div
                v-if="!props.no_header"
                class="flex border-b border-slate-300 items-center"
                :class="{ 'py-4 px-4 lg:px-6': props.set_margins }"
              >
                <!-- Close button -->
                <fwb-button
                  v-if="!props.title"
                  @click="setModalOpen(false)"
                  color="light"
                  pill
                  square
                  size="lg"
                  class="hidden lg:inline-block bg-white border-white text-slate-400 hover:text-slate-900 lg:top-[calc(50% - 1rem)]"
                  :class="[(max_width !== 'full' && !left_close_inside) ? 'absolute lg:-left-4' : 'lg:ml-4'].join('')"
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
                <!-- Seprator on full width -->
                <div
                  v-if="max_width === 'full'"
                  class="hidden lg:flex w-[1px] bg-slate-300 h-9 ml-4"
                ></div>
                <!-- Mobile Close button -->
                <fwb-button
                  v-if="!hide_header_close_on_mobile && !props.title"
                  @click="setModalOpen(false)"
                  color="light"
                  pill
                  square
                  size="lg"
                  class="p-0 mr-2 lg:hidden bg-white border-white text-slate-400 hover:text-slate-900 lg:top-[calc(50% - 1rem)]"
                  :class="[!set_margins ? 'ml-3' : ''].join(' ')"
                >
                  <IconArrowLeft width="24" height="24" class="ml-2" />
                </fwb-button>
                <div class="w-full">
                  <div
                    v-if="props.title"
                    class="flex items-center justify-between"
                    :class="{ 'px-4 py-4': !props.set_margins }"
                  >
                    <span class="text-lg font-semibold text-slate-900">{{ props.title }}</span>
                    <!-- Close button -->
                    <fwb-button
                      v-if="props.title"
                      @click="setModalOpen(false)"
                      color="light"
                      pill
                      square
                      size="lg"
                      class="bg-white border-white text-slate-400 hover:text-slate-900"
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
