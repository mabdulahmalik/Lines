<template>
    <div class="bg-slate-50 h-full">
    <TopbarArea />
    <div class="flex w-full overflow-hidden pt-[64px] lg:pt-20 duration-300 ease-linear">
      <SidebarArea class="w-sidebar"/>
      <main class="h-full flex-1 duration-300 ease-linear" :class="{'lg:ml-sidebar': sidebarStore.isLgSidebarOpen}">
        <router-view v-slot="{ Component }">
          <transition name="fade">
            <component :is="Component" :key="$route.path"/>
          </transition>
        </router-view>
      </main>
    </div>
  </div>
  </template>
  
  <script setup lang="ts">
  import { RouterView } from 'vue-router';
  import { useSidebarStore } from '@/stores/sidebar';
  import TopbarArea from '@/components/layout/Topbar/TopbarArea.vue';
  import SidebarArea from '@/components/layout/Sidebar/SidebarArea.vue';
  
  const sidebarStore = useSidebarStore();
  </script>
  