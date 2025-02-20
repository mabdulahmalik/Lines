<template>
    <fwb-dropdown>
      <template #trigger>
        <slot name="trigger"></slot>
      </template>
      <slot></slot>
    </fwb-dropdown>
  </template>
  
  <script setup>
    import { FwbDropdown} from 'flowbite-vue'
  </script>

  <style scoped>
  :deep(.absolute.z-10.rounded){
    border-radius: 0.5rem !important;
    z-index: 999;
  }
  </style>

