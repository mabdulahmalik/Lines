<script lang="ts" setup>
import { RouterLink } from 'vue-router';
import { useBreadcrumbStore } from '@/stores/breadcrumb'

type Size = 'sm' | 'base' | 'lg' | 'xl';

const props = withDefaults(
  defineProps<{
    size?: Size;
    color?: string;
  }>(),
  {
    size: 'lg',
    color: 'white'
  }
);
const breadcrumbStore = useBreadcrumbStore();

</script>
<template>
  <nav class="flex" aria-label="Breadcrumb">
    <ol class="inline-flex items-center space-x-1 md:space-x-3">
      <li
        v-for="(breadcrumb, index) in breadcrumbStore.breadcrumbs"
        :class="`inline-flex items-center font-semibold transition-all duration-300 ease-in-out text-${props.size}`"
        :key="index"
      >
        <template v-if="breadcrumb.to">
          <RouterLink :to="breadcrumb.to" :class="`text-${color}`">{{ breadcrumb.title }}</RouterLink>
          <span :class="`ml-1 text-${color}`">/</span>
        </template>
        <template v-else>
          <span :class="`${(color == 'white') ? 'text-brand-200' : 'text-slate-500'}`">{{ breadcrumb.title }}</span>
        </template>
      </li>
    </ol>
  </nav>
</template>
