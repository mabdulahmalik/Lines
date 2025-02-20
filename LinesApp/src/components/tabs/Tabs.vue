<script setup lang="ts">
import { ref, watch } from 'vue';
import { FwbBadge } from 'flowbite-vue';
import { TabType } from '@/types/tab';

const props = defineProps<{
  tabs?: TabType[];
  margin?: string;
  activeTab?: number | undefined;
}>();

// Reactive currentTab initialized with activeTab or 0
const currentTab = ref(props.activeTab ?? 0);

function setActiveTab(index: number) {
  currentTab.value = index;
}

// Watch for changes in the activeTab prop
watch(
  () => props.activeTab,
  (newVal) => {
    if (newVal !== undefined && newVal !== currentTab.value) {
      currentTab.value = newVal;
    }
  }
);
</script>

<template>
  <div>
    <!-- Tab Navigation -->
    <div class="border-b border-slate-300" :class="margin">
      <ul
        class="flex flex-wrap -mb-px text-sm font-medium text-center text-slate-500 gap-2 lg:gap-5"
      >
        <li v-for="(tab, index) in tabs" :key="index" class="lg:me-6">
          <a
            href="#"
            :class="[
              'inline-flex items-center justify-center pb-4 pt-1 px-1 border-b-[3px] rounded-t-lg duration-300 ease-in-out',
              currentTab === index
                ? 'text-brand-600 border-brand-600'
                : 'border-transparent hover:text-slate-700 hover:border-slate-300',
              tab.disabled ? 'cursor-not-allowed opacity-50 pointer-events-none' : 'cursor-pointer',
            ]"
            @click.prevent="setActiveTab(index)"
          >
            <span v-if="tab.icon" class="me-1 lg:me-2">
              <!-- Check if icon is a string (HTML) -->
              <span v-if="typeof tab.icon === 'string'" v-html="tab.icon"></span>
              <!-- Render icon as a component if it's a Vue component -->
              <component v-else :is="tab.icon" />
            </span>
            <span v-if="tab.label">{{ tab.label }}</span>
            <span v-if="tab.badge" class="ml-2">
              <fwb-badge type="dark" class="mr-0">{{ tab.badge }}</fwb-badge>
            </span>
          </a>
        </li>
      </ul>
    </div>

    <!-- Tab Content with Transition -->
    <transition name="fade" mode="out-in">
      <div :key="currentTab">
        <slot :name="`tab-${currentTab}`"></slot>
      </div>
    </transition>
  </div>
</template>
