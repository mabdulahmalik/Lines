<script setup lang="ts">
import { RouterLink } from 'vue-router';

</script>

<template>
  <header class="w-full">
    <div class="flex items-center justify-center w-full p-4 h-16 lg:h-20 fixed bg-header-gradient z-[51]">
        <router-link to="/">
            <img src="/lines-logo-white.svg" class="h-8 sm:h-11" alt="Lines Logo" />
        </router-link>
    </div>
  </header>
</template>

<style scoped></style>
