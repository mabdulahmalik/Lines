<script setup lang="ts">
import { ref } from 'vue';
import { FwbBadge } from 'flowbite-vue';
import { TabType } from '@/types/tab';

defineProps<{
  tabs?: TabType[];
  min_width?: string;
  full_height?: boolean;
}>();

const activeTab = ref(0);

function setActiveTab(index: number) {
  activeTab.value = index;
}
</script>

<template>
  <div class="flex" :class="{ 'h-full': full_height }">
    <!-- Tab Navigation -->
    <ul
      class="flex flex-col text-sm font-medium text-slate-900 bg-slate-50 pl-px"
      :class="`min-w-${min_width}`"
    >
      <li v-for="(tab, index) in tabs" :key="index" class="">
        <a
          href="#"
          :class="[
            'inline-flex items-center p-4 border-l-[4px] duration-300 ease-in-out w-full',
            activeTab === index
              ? 'border-brand-600 bg-white'
              : 'border-transparent hover:text-slate-700 hover:border-slate-300 bg-slate-50',
          ]"
          @click.prevent="setActiveTab(index)"
        >
          <span v-if="tab.icon" v-html="tab.icon" class="me-2"></span>

          <div v-if="tab.label || tab.sub_label">
            <div v-if="tab.label">
              {{ tab.label }}
            </div>
            <div v-if="tab.sub_label" class="text-brand-600">{{ tab.sub_label }}</div>
          </div>

          <span v-if="tab.badge" class="ml-2">
            <fwb-badge type="dark">{{ tab.badge }}</fwb-badge>
          </span>
        </a>
      </li>
    </ul>
    <!-- <div class="border-b border-slate-300">
    </div> -->

    <!-- Tab Content with Transition -->
    <div class="flex-1">
      <transition name="fade" mode="out-in">
        <div :key="activeTab">
          <slot :name="`tab-${activeTab}`"></slot>
        </div>
      </transition>
    </div>
  </div>
</template>
