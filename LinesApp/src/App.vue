<script setup lang="ts">
import { RouterView } from 'vue-router';
import { useSidebarStore } from '@/stores/sidebar';
import SidebarArea from './components/layout/Sidebar/SidebarArea.vue';
import TopbarArea from './components/layout/Topbar/TopbarArea.vue';
import { useModalStore } from './stores/modal';
import NotificationPopup from './views/common/NotificationPopup.vue';

const sidebarStore = useSidebarStore();
const modalStore = useModalStore();
</script>

<template>
  <div class="bg-slate-50 h-full">
    <TopbarArea />
    <div class="flex w-full h-full overflow-hidden pt-[64px] lg:pt-20 duration-300 ease-linear">
      <SidebarArea class="w-sidebar"/>
      <main class="h-full flex-1 duration-300 ease-linear" :class="{'lg:ml-sidebar lg:max-w-[calc(100%-256px)]': sidebarStore.isLgSidebarOpen}">
        <!-- dynamic modal component here -->
        <component v-if="modalStore.globalModal" :is="modalStore.globalModal" />
        <router-view v-slot="{ Component }">
          <transition name="fade" mode="out-in">
            <component :is="Component" :key="$route.path"/>
          </transition>
        </router-view>
      </main>
    </div>
    <NotificationPopup />
  </div>
</template>

<style lang="css">
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
