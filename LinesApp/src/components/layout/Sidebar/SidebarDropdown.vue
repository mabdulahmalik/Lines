<script setup lang="ts">
import { FwbBadge } from 'flowbite-vue';
import { useSidebarStore } from '@/stores/sidebar';
import { ref } from 'vue';

const sidebarStore = useSidebarStore();

const props = defineProps(['items', 'page', 'currentPage']);
// console.log(props.currentPage)

const items = ref(props.items);

const handleItemClick = (index: number) => {
  const pageName =  props.items[index].label;
  sidebarStore.selected = pageName;
  // close sidebar
  sidebarStore.isSidebarOpen = false;
};
</script>

<template>
  <ul class="pb-5 flex flex-col pl-6">
    <template v-for="(childItem, index) in items" :key="index">
      <li
        class="border-l-4 border-brand-200 duration-300 ease-in-out"
        :class="{
          '!border-brand-700': currentPage === childItem.label,
        }"
      >
        <router-link
          :to="childItem.route"
          @click="handleItemClick(index)"
          class="group relative flex items-center gap-2.5 rounded-md py-3 pr-2 pl-5 font-medium text-bodydark2 duration-300 ease-in-out"
          :class="{
            '!text-brand-700': currentPage === childItem.label,
          }"
        >
          {{ childItem.label }}
          <fwb-badge
            v-if="childItem.badge > 0"
            class="ml-auto bg-white px-1 text-slate-600 min-w-6"
            :class="{
              '!text-brand-700': currentPage === childItem.label,
            }"
            >{{ childItem.badge }}</fwb-badge
          >
        </router-link>
      </li>
    </template>
  </ul>
</template>
