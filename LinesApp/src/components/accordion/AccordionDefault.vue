<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { FwbBadge } from 'flowbite-vue';
import { Priority } from '@/types/jobs';
import { JobStatus } from '@/api/__generated__/graphql';
import { EncounterPriority } from '@/api/__generated__/graphql';
// import { Priority } from '@/types/jobs';
type chartingBadgeType = null | 'For me' | 'Free to grab';
const accordionOpen = ref<boolean>(false);

const emit = defineEmits(['open', 'close']);
const props = withDefaults(
  defineProps<{
    id: string;
    badge?: Priority | JobStatus | EncounterPriority | chartingBadgeType | string;
    title?: string;
    count?: number | string;
    active?: boolean;
    def_header?: boolean;
    custom_header?: boolean;
  }>(),
  {
    badge: EncounterPriority.Normal,
    active: false,
    def_header: false,
    custom_header: false,
  }
);

watch(
  () => props.active,
  (value) => {
    accordionOpen.value = value;
  },
  { deep: true }
);
watch(accordionOpen, (value) => {
  if (value) {
    emit('open', value);
  } else {
    emit('close');
  }
});
onMounted(() => {
  accordionOpen.value = props.active;
});
</script>

<template>
  <div class="flex flex-col">
    <div
      v-if="props.custom_header"
      :id="`accordion-title-${id}`"
      class="flex items-center justify-between w-full text-left font-semibold py-2 gap-2 cursor-pointer"
      @click.prevent="accordionOpen = !accordionOpen"
      :aria-expanded="accordionOpen"
      :aria-controls="`accordion-text-${id}`"
    >
      <slot name="customHeader" :open="accordionOpen"></slot>
    </div>

    <div
      v-else
      :id="`accordion-title-${id}`"
      class="flex items-center justify-between w-full text-left font-semibold py-2 gap-2 cursor-pointer"
      @click.prevent="accordionOpen = !accordionOpen"
      :aria-expanded="accordionOpen"
      :aria-controls="`accordion-text-${id}`"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="24"
        height="24"
        viewBox="0 0 24 24"
        fill="none"
      >
        <path
          class="transform origin-center transition duration-200 ease-out"
          :class="{ 'rotate-0': accordionOpen, 'rotate-180': !accordionOpen }"
          d="M18 15L12 9L6 15"
          stroke="#64748B"
          stroke-width="1.5"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
      <div class="flex items-center gap-2 w-full">
        <template v-if="def_header">
          <fwb-badge
            size="sm"
            class="rounded text-sm font-semibold m-0"
            :class="{
              'text-white bg-radical-red-600': badge === 'STAT',
              'text-white bg-turquoise-blue-500': badge === 'NORMAL',
              'text-white bg-blue-700': badge === 'ROUTINE',
              'text-slate-700 bg-slate-100': badge === 'CANCELED',
              'text-green-700 bg-green-100': badge === 'COMPLETED' || badge === 'SCHEDULED',
              'text-brand-700 bg-brand-100': badge === 'UNDERWAY',
              'text-slate-900 bg-white': badge === 'For me' || badge === 'Free to grab',
            }"
          >
            <template v-if="badge === 'STAT'" #icon>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="16"
                height="16"
                viewBox="0 0 16 16"
                fill="none"
                class="mr-1"
              >
                <path d="M4 14V10" stroke="white" stroke-width="2" stroke-linecap="round" />
                <path d="M8 14V6" stroke="white" stroke-width="2" stroke-linecap="round" />
                <path d="M12 14V2" stroke="white" stroke-width="2" stroke-linecap="round" />
              </svg>
            </template>
            <template v-if="badge === 'NORMAL'" #icon>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="16"
                height="16"
                viewBox="0 0 16 16"
                fill="none"
                class="mr-1"
              >
                <path d="M4 14V10" stroke="white" stroke-width="2" stroke-linecap="round" />
                <path d="M8 14V6" stroke="white" stroke-width="2" stroke-linecap="round" />
              </svg>
            </template>
            <template v-if="badge === 'ROUTINE'" #icon>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="16"
                height="16"
                viewBox="0 0 16 16"
                fill="none"
                class="mr-1"
              >
                <path d="M4 14V10" stroke="white" stroke-width="2" stroke-linecap="round" />
              </svg>
            </template>
            {{ badge }}
          </fwb-badge>
          <div class="flex-1 bg-[#CBD5E1] h-[1px]"></div>
          <fwb-badge type="dark" size="sm" class="rounded text-xs font-semibold m-0">
            {{ count }}
          </fwb-badge>
        </template>
        <span v-else-if="title" class="text-base font-semibold">{{ title }}</span>
        <slot v-else name="header"> </slot>
      </div>
    </div>
    <div
      :id="`accordion-text-${id}`"
      role="region"
      :aria-labelledby="`accordion-title-${id}`"
      class="grid text-slate-600 transition-all duration-300 ease-in-out"
      :class="
        accordionOpen ? 'grid-rows-[1fr] opacity-100' : 'grid-rows-[0fr] opacity-0 overflow-hidden'
      "
    >
      <div class="duration-300 ease-in-out" :class="{ 'overflow-hidden': !accordionOpen }">
        <div class="flex flex-col" :class="{ 'pb-8 gap-4': def_header, 'pb-2': !def_header }">
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>
