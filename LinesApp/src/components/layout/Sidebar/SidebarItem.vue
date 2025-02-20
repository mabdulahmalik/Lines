<script setup lang="ts">
import { useSidebarStore } from '@/stores/sidebar';
import { RouteRecordNameGeneric, useRoute } from 'vue-router';
import SidebarDropdown from './SidebarDropdown.vue';
import { IconChevronDown } from "@/components/icons/index";

const sidebarStore = useSidebarStore();
const route = useRoute();

const props = defineProps(['item', 'index']);
interface SidebarItem {
  label: string;
}
const handleItemClick = () => {
  const pageName = sidebarStore.page === props.item.label ? '' : props.item.label;
  // const pageName =  props.item.label
  sidebarStore.page = pageName;

  if (props.item.children) {
    return props.item.children.some((child: SidebarItem) => sidebarStore.selected === child.label);
  }

  else{
    sidebarStore.isSidebarOpen = false
  }
};

const isLinkActive = (link: { children: any[]; label: RouteRecordNameGeneric }) => {
  if (link.children) {
    return route.matched.some(({ name }) => link.children.some((c) => c.label === name));
  } 
  // else {
  //   return route.matched.some(({ name }) => name === link.label);
  // }
};



// Transitions
const enter = (el: Element, done: () => void) => {
  const element = el as HTMLElement;
  const transitionEndHandler = () => {
    element.removeEventListener('transitionend', transitionEndHandler);
    done();
  };
  element.addEventListener('transitionend', transitionEndHandler);
  element.style.maxHeight = element.scrollHeight + 'px';
};

const leave = (el: Element, done: () => void) => {
  const element = el as HTMLElement;
  const transitionEndHandler = () => {
    element.removeEventListener('transitionend', transitionEndHandler);
    done();
  };
  element.addEventListener('transitionend', transitionEndHandler);
  element.style.maxHeight = element.scrollHeight + 'px';
  requestAnimationFrame(() => {
    element.style.maxHeight = '0px';
  });
};
</script>

<template>
  <li
    class="duration-300 ease-in-out"
    :class="{
      'bg-brand-100 rounded-lg active': sidebarStore.page.toLowerCase() === item.label.toLowerCase() || isLinkActive(item),
    }"
  >
    <router-link
      :to="item.route"
      class="group relative flex items-center gap-2.5 rounded-md p-3 font-semibold text-base duration-300 ease-in-out"
      @click.prevent="handleItemClick"
      :class="{
        'bg-brand-50 text-brand-700': sidebarStore.page.toLowerCase() === item.label.toLowerCase() || isLinkActive(item),
      }"
    >
      <span v-html="item.icon"></span>

      {{ item.label }}

      <IconChevronDown
        v-if="item.children"
        width="20"
        height="20"
        :strock-width="0.8"
        class="absolute right-4 top-1/2 -translate-y-1/2"
        :class="{ 'rotate-180': sidebarStore.page === item.label }"
      />
    </router-link>

    <!-- Dropdown Menu Start -->
    <transition name="menu" @enter="enter" @leave="leave">
    <div
      class="overflow-hidden rounded-b-lg "
      :class="{'max-h-72': sidebarStore.page === item.label, 'max-h-0': sidebarStore.page !== item.label}"
      v-show="sidebarStore.page.toLowerCase() === item.label.toLowerCase()"
    >
        <SidebarDropdown
          v-if="item.children"
          :items="item.children"
          :currentPage="route.name"
          :page="item.label"
        />
      </div>
    </transition>
    <!-- Dropdown Menu End -->
  </li>
</template>

<style scoped>
/* Transition for the dropdown menu */
.menu-enter-active,
.menu-leave-active {
  transition: max-height 0.5s ease-in-out;
}
.menu-enter-from,
.menu-leave-to {
  max-height: 0;
}

</style>
