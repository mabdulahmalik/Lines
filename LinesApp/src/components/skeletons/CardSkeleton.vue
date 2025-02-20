<script setup>
import SkeletonItem from './SkeletonItem.vue';
</script>

<template>
  <div class="w-full rounded-xl bg-slate-100 border border-slate-200 p-4 overflow-hidden">
    <div class="flex lg:flex-row gap-3 flex-col justify-between lg:items-center">
      <div class="flex flex-col gap-2 flex-1">
        <SkeletonItem backgroundColor="#CBD5E1" class="w-24 h-4 rounded-3xl" />
        <SkeletonItem class="md:w-4/6 w-4/5 h-4 rounded-3xl" />
      </div>
      <div class="flex items-center gap-2 lg:mt-0 mt-2">
        <SkeletonItem class="w-16 h-6 rounded-2xl" />
        <SkeletonItem backgroundColor="#CBD5E1" class="w-8 h-8 rounded-full" />
      </div>
    </div>
    <div>
      <div class="my-4">
        <div class="flex gap-1">
          <SkeletonItem backgroundColor="#CBD5E1" class="w-10 h-4 rounded-3xl" />
          <SkeletonItem backgroundColor="#CBD5E1" class="w-[136px] h-4 rounded-3xl" />
        </div>
        <SkeletonItem class="md:w-[45%] w-3/5 h-4 rounded-3xl mt-2" />
      </div>
      <SkeletonItem backgroundColor="#CBD5E1" class="w-20 h-4 rounded-3xl" />
    </div>
  </div>
</template>
