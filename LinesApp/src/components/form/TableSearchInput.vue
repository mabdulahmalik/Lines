<script setup lang="ts">
import { ref, computed, watch, nextTick } from 'vue';
import { FwbInput } from 'flowbite-vue';
import { IconSearchOutline } from '@/components/icons/index';

const props = defineProps({
  placeholder: {
    type: String,
    default: 'Search...',
  },
  options: {
    type: Array as () => { value: string; property: string }[],
    default: () => [],
  },
  modelValue: {
    type: String,
    default: '',
  },
});
const emit = defineEmits(['update:modelValue', 'search']);

const inputValue = ref(props.modelValue);
const loading = ref(false);
const showDropdown = ref(false);

const filteredResults = computed(() => {
  const seen = new Set();
  return props.options.filter((option) => {
    const lowerCaseValue = option.value?.toLowerCase();
    if (
      lowerCaseValue &&
      lowerCaseValue.includes(inputValue.value.toLowerCase()) &&
      !seen.has(lowerCaseValue)
    ) {
      seen.add(lowerCaseValue);
      return true;
    }
    return false;
  });
});

watch(inputValue, (newValue) => {
  emit('update:modelValue', newValue);
  showDropdown.value = !!newValue;
});

const onInput = () => {
  loading.value = true;
  nextTick(() => {
    loading.value = false;
  });
};

const onUpdate = (value: string) => {
  if (value === '') {
    triggerSearch();
  }
};

const triggerSearch = () => {
  loading.value = true;
  emit('search', inputValue.value);
  nextTick(() => {
    loading.value = false;
    showDropdown.value = false;
  });
};

const selectResult = (result: string) => {
  inputValue.value = result;
  triggerSearch();
};
</script>
<template>
  <div class="relative">
    <div class="flex items-center">
      <fwb-input
        v-model="inputValue"
        @input="onInput"
        @keydown.enter="triggerSearch"
        @update:modelValue="onUpdate"
        type="text"
        :placeholder="placeholder"
        size="sm"
        class="mx-px w-full"
      >
        <template #prefix>
          <button
            @click="triggerSearch"
            :disabled="loading"
            type="button"
            class="pointer-events-auto"
          >
            <IconSearchOutline
              v-if="!loading"
              width="20"
              height="20"
              class="text-gray-500 dark:text-gray-400"
            />
            <svg
              v-else
              class="animate-spin h-5 w-5 text-gray-500"
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
            >
              <path
                d="M24 12C24 18.6274 18.6274 24 12 24C5.37258 24 0 18.6274 0 12C0 5.37258 5.37258 0 12 0C18.6274 0 24 5.37258 24 12ZM2.17955 12C2.17955 17.4237 6.57631 21.8205 12 21.8205C17.4237 21.8205 21.8205 17.4237 21.8205 12C21.8205 6.57631 17.4237 2.17955 12 2.17955C6.57631 2.17955 2.17955 6.57631 2.17955 12Z"
                fill="#E2E8F0"
              />
              <path
                d="M22.5522 9.22803C23.1343 9.07511 23.487 8.47699 23.2819 7.91114C22.8704 6.77566 22.289 5.70681 21.556 4.74172C20.6028 3.48681 19.4118 2.43192 18.051 1.6373C16.6901 0.842669 15.1861 0.323864 13.6247 0.1105C12.424 -0.0535871 11.2074 -0.0345545 10.0163 0.165098C9.4227 0.264591 9.07511 0.86567 9.22803 1.44779C9.38095 2.0299 9.97664 2.37142 10.5721 2.28391C11.4843 2.14986 12.4126 2.14466 13.3296 2.26998C14.6074 2.44459 15.8383 2.86916 16.9519 3.51946C18.0656 4.16976 19.0403 5.03305 19.8204 6.06003C20.3802 6.7971 20.8319 7.60812 21.1635 8.4684C21.3799 9.03 21.9701 9.38095 22.5522 9.22803Z"
                fill="#5E4DB5"
              />
            </svg>
          </button>
        </template>
      </fwb-input>
    </div>
    <div
      v-if="showDropdown"
      class="absolute right-0 bg-white rounded-md border border-gray-200 shadow-sm p-1 mt-1 w-full z-50 min-w-80"
    >
      <ul>
        <li
          v-for="(result, index) in filteredResults"
          :key="index"
          @click="selectResult(result.value)"
          class="px-4 py-2 cursor-pointer hover:bg-brand-50 text-sm font-medium text-slate-900 rounded-lg"
        >
          <div class="flex gap-2 items-center justify-between">
            {{ result.value }}
            <span class="uppercase text-brand-600 text-xs font-semibold">{{
              result.property
            }}</span>
          </div>
        </li>
        <li v-if="filteredResults.length === 0" class="px-4 py-6 text-sm text-gray-900">
          <div class="flex flex-col items-center justify-center text-center">
            <span class="p-4 bg-slate-100 rounded-full">
              <IconSearchOutline width="20" height="20" />
            </span>
            <div class="font-semibold">No search results found</div>
            <div class="font-medium text-slate-500">Please try again with a different query</div>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>
