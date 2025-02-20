<template>
    <div ref="dropdownRef" class="relative inline-block text-left">
      <div @click="toggleDropdown">
        <slot name="trigger"></slot>
      </div>
      <teleport to="body">
        <transition name="fade">
          <div
            v-if="isOpen"
            ref="dropdownMenuRef"
            :class="['absolute', 'mt-2 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none z-50']"
            :style="dropdownStyles"
          >
            <slot></slot>
          </div>
        </transition>
      </teleport>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted, onBeforeUnmount, watch, nextTick } from 'vue';
  
  const props = defineProps({
    alignment: {
      type: String,
      default: 'right',
      validator: value => ['left', 'right', 'center'].includes(value),
    },
    placement: {
      type: String,
      default: 'bottom',
      validator: value => ['top', 'bottom'].includes(value),
    },
  });
  
  const isOpen = ref(false);
  const dropdownRef = ref(null);
  const dropdownMenuRef = ref(null);
  const dropdownStyles = ref({});
  
  const toggleDropdown = async () => {
    isOpen.value = !isOpen.value;
    if (isOpen.value) {
      await nextTick();
      updateDropdownPosition();
    }
  };
  
  const handleClickOutside = (event) => {
    if (
      dropdownRef.value &&
      !dropdownRef.value.contains(event.target) &&
      dropdownMenuRef.value &&
      !dropdownMenuRef.value.contains(event.target)
    ) {
      isOpen.value = false;
    }
  };
  
  const updateDropdownPosition = () => {
    if (!dropdownRef.value || !dropdownMenuRef.value) return;
  
    const rect = dropdownRef.value.getBoundingClientRect();
    const menuHeight = dropdownMenuRef.value.offsetHeight;
    const menuWidth = dropdownMenuRef.value.offsetWidth;
  
    let top;
    let left;
  
    // Determine vertical placement
    if (props.placement === 'top') {
      if (rect.top - menuHeight < 0) {
        top = `${rect.bottom}px`;
      } else {
        top = `${rect.top - menuHeight}px`;
      }
    } else {
      if (rect.bottom + menuHeight > window.innerHeight) {
        top = `${rect.top - menuHeight}px`;
      } else {
        top = `${rect.bottom}px`;
      }
    }
  
    // Determine horizontal alignment
    if (props.alignment === 'left') {
      if (rect.left + menuWidth > window.innerWidth) {
        left = `${rect.right - menuWidth}px`;
      } else {
        left = `${rect.left}px`;
      }
    } else if (props.alignment === 'center') {
      const centerLeft = rect.left + rect.width / 2 - menuWidth / 2;
      if (centerLeft < 0) {
        left = '0px';
      } else if (centerLeft + menuWidth > window.innerWidth) {
        left = `${window.innerWidth - menuWidth}px`;
      } else {
        left = `${centerLeft}px`;
      }
    } else {
      if (rect.right - menuWidth < 0) {
        left = `${rect.left}px`;
      } else {
        left = `${rect.right - menuWidth}px`;
      }
    }
  
    dropdownStyles.value = {
      top,
      left,
    };
  };
  
  onMounted(() => {
    document.addEventListener('click', handleClickOutside);
    window.addEventListener('resize', updateDropdownPosition);
  });
  
  onBeforeUnmount(() => {
    document.removeEventListener('click', handleClickOutside);
    window.removeEventListener('resize', updateDropdownPosition);
  });
  
  watch(isOpen, (newVal) => {
    if (newVal) {
      updateDropdownPosition();
    }
  });
  </script>
  
  <style scoped>
  .fade-enter-active, .fade-leave-active {
    transition: opacity 0.5s;
  }
  .fade-enter-from, .fade-leave-to {
    opacity: 0;
  }
  </style>
  