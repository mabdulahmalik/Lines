<script setup lang="ts">
const props = defineProps<{
  photos: any[];
}>();
</script>

<template>
  <div
    class="font-medium text-sm p-4 rounded-lg bg-slate-50 flex md:justify-normal justify-between md:gap-16 flex-wrap"
  >
    <div class="flex flex-wrap gap-2">
      <div
        v-for="(photo, index) in props.photos"
        :key="index"
        class="rounded bg-[#D9D9D9] h-10 w-[75px] overflow-hidden"
      >
        <img
          class="w-full h-full object-cover"
          :src="photo.Url"
          :alt="photo.FileName"
        />
      </div>
    </div>
  </div>
</template>
