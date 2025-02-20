<script setup lang="ts">
    const props =
    defineProps<{
        to?: string;
    }>();
</script>

<template>
    <li v-if="props.to" class="rounded text-sm font-medium text-slate-900 duration-300 ease-in-out hover:bg-brand-50">
        <router-link
          :to="props.to"
          class="flex items-center w-full py-2 px-4"
        >
            <slot></slot>
        </router-link>
    </li>
    <li v-else class="flex items-center rounded font-medium text-sm text-slate-900 py-2 px-4 duration-300 ease-in-out hover:bg-brand-50 cursor-pointer">
        <slot></slot>
    </li>

</template>