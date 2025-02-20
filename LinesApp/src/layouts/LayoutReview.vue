<template>
    <div class="bg-slate-50 h-full">
      <LayoutReviewTopbar />
      <main class="pt-[64px] lg:pt-20">
        <router-view v-slot="{ Component }">
          <transition name="fade">
            <component :is="Component" :key="$route.path"/>
          </transition>
        </router-view>
      </main>
    </div>
  </template>
  
  <script setup lang="ts">
  import { RouterView } from 'vue-router';
  import LayoutReviewTopbar from '@/components/layout/Topbar/LayoutReviewTopbar.vue';
  </script>
  