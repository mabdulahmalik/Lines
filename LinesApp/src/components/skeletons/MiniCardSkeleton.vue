<script setup>
import SkeletonItem from './SkeletonItem.vue';
</script>

<template>
  <div
    class="w-full rounded-xl bg-slate-100 border border-slate-200 p-4 overflow-hidden flex flex-col gap-4"
  >
    <div class="flex flex-col gap-2">
      <SkeletonItem backgroundColor="#CBD5E1" class="w-24 h-4 rounded-3xl" />
      <SkeletonItem class="w-full h-4 rounded-3xl" />
    </div>

    <div class="flex items-center gap-2">
      <SkeletonItem class="w-14 h-6 rounded-2xl" />
      <SkeletonItem backgroundColor="#CBD5E1" class="w-8 h-8 rounded-full" />
    </div>

    <div>
      <div class="flex gap-2">
        <SkeletonItem backgroundColor="#CBD5E1" class="w-10 h-4 rounded-3xl" />
        <SkeletonItem backgroundColor="#CBD5E1" class="w-[136px] h-4 rounded-3xl" />
      </div>
      <SkeletonItem class="w-4/5 h-4 rounded-3xl mt-2" />
    </div>
    <SkeletonItem backgroundColor="#CBD5E1" class="w-20 h-4 rounded-3xl" />
  </div>
</template>
