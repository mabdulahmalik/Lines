<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import { onClickOutside } from '@vueuse/core';

const props = withDefaults(
  defineProps<{
    modelValue?: string;
    label?: string;
    options?: { value: string; name: string, label?: string; }[];
    create_option?: boolean;
    placeholder?: string;
    error?: boolean;
    error_message?: string;
  }>(),
  {
    options: () => [],
    placeholder: 'Type to search...',
    error: false,
    error_message: 'Please text in here',
    create_option: false,
  }
);

const emits = defineEmits(['update:modelValue', 'create']);

const query = ref<string>(props.modelValue || '');
const filteredOptions = ref<{ value: string; name: string, label?: string; }[]>([]);
const showDropdown = ref<boolean>(false);
const loading = ref<boolean>(false);
const optionSelected = ref<boolean>(false); // Track if an option was selected or created
const isUserInput = ref<boolean>(false); // Track if the input was manually changed

const filterOptions = (): void => {
  loading.value = true;
  if (query.value.trim() === '') {
    filteredOptions.value = [];
    loading.value = false;
  } else {
    setTimeout(() => {
      filteredOptions.value = props.options.filter((option) =>
        option.name.toLowerCase().includes(query.value.toLowerCase())
      );
      loading.value = false;
    }, 500); // Simulate a delay for loading
  }
};

const selectOption = (option: { value: string; name: string }): void => {
  query.value = option.name;
  showDropdown.value = false;
  optionSelected.value = true; // Option selected
  isUserInput.value = false; // Reset flag on option selection
  emits('update:modelValue', option.value);
};

const clearInput = (): void => {
  query.value = '';
  filteredOptions.value = [];
  showDropdown.value = false;
  optionSelected.value = false; // Reset selection tracking
  isUserInput.value = false; // Reset flag on clearing input
  emits('update:modelValue', '');
};

const isQueryUnique = computed(() => {
  const queryLower = query.value.trim().toLowerCase();
  return (
    queryLower !== '' &&
    !props.options.map((option) => option.name.toLowerCase()).includes(queryLower)
  );
});

const target = ref(null);

onClickOutside(target, () => {
  showDropdown.value = false;

  // Only clear the input if the user manually changed it and no option was selected
  if (!optionSelected.value && isUserInput.value) {
    query.value = '';
    isUserInput.value = false; // Reset flag
    emits('update:modelValue', '');
  }
});

// Track manual input changes
const onInput = () => {
  isUserInput.value = true; // Mark that user changed the input
  filterOptions(); // Re-filter the options as the query changes
};

const createOption = (val: string) => {
  emits('create', val);
  console.log('Creating option:', val);
  optionSelected.value = true; // Option created
  showDropdown.value = false;
  isUserInput.value = false; // Reset flag on option creation
};

watch(
  () => props.modelValue,
  (newValue) => {
    const selectedOption = props.options.find(option => option.value === newValue);
    if (selectedOption) {
      query.value = selectedOption.name;
      isUserInput.value = false; // Reset flag when prop value changes
    }
  },
  { immediate: true }
);

// Watch for changes in props.options and refilter when necessary
watch(
  () => props.options,
  () => {
    filterOptions();
  },
  { deep: true }
);

watch(query, filterOptions); // Automatically filter options when the query changes

filterOptions(); // Initial filter call
defineExpose({ selectOption, clearInput, createOption });
</script>


<template>
  <div class="relative">
    <span v-if="label" class="mb-2 block text-sm font-medium text-gray-900 dark:text-white">{{
      label
    }}</span>
    <div class="relative" ref="target">
      <input
        v-model="query"
        type="text"
        class="pr-10 border text-gray-900 rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 text-sm"
        :class="{
          'bg-radical-red-50 border-radical-red-500': error,
          'bg-gray-50 border-gray-300': !error,
        }"
        @focus="showDropdown = true"
        @input="onInput"
        :placeholder="placeholder"
      />
      <div class="absolute top-[50%] right-0 -mt-3 mr-2 flex items-center space-x-2 h-6">
        <svg
          v-if="query && !loading"
          class="h-5 w-5 cursor-pointer text-gray-500 hover:text-gray-700"
          @click="clearInput"
          xmlns="http://www.w3.org/2000/svg"
          width="16"
          height="16"
          viewBox="0 0 16 16"
          fill="none"
        >
          <path
            d="M12 4L4 12M4 4L12 12"
            stroke="currentColor"
            stroke-width="1.5"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
        </svg>
        <svg
          v-if="loading"
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
      </div>
      <ul
        v-if="showDropdown && filteredOptions.length && !create_option"
        class="absolute w-full mt-1 bg-white border border-slate-200 rounded-lg shadow-sm p-1 z-[99]"
      >
        <li class="px-2 pt-2 pb-0.5">
          <span class="text-xs font-medium text-slate-500"> Select an option </span>
        </li>
        <ul class="max-h-48 overflow-y-auto custom-scroll">
          <li
            v-for="(option, index) in filteredOptions"
            :key="index"
            @click="selectOption(option)"
            class="px-4 py-2 cursor-pointer hover:bg-brand-50 text-sm font-medium text-slate-900 rounded-lg flex items-center justify-between"
          >
            {{ option.name }}
            <span v-if="option.label" class="text-brand-600 text-xs font-semibold">{{ option.label }}</span>
          </li>
        </ul>
      </ul>
      <ul
        v-if="(isQueryUnique && create_option && showDropdown) || (showDropdown && filteredOptions.length)"
        class="absolute w-full mt-1 bg-white border border-slate-200 rounded-lg shadow-sm p-1 z-[999]"
      >
        <li class="px-2 pt-2 pb-0.5">
          <span class="text-xs font-medium text-slate-500">
            Select an option<span v-if="isQueryUnique && props.create_option">&nbsp;or create one </span>
          </span>
        </li>
        <li>
          <ul class="max-h-48 overflow-y-auto custom-scroll">
            <li
              v-for="(option, index) in filteredOptions"
              :key="index"
              @click="selectOption(option)"
              class="px-4 py-2 cursor-pointer hover:bg-brand-50 text-sm font-medium text-slate-900 rounded-lg flex items-center justify-between"
            >
              {{ option.name }}
              <span v-if="option.label" class="text-brand-600 text-xs font-semibold">{{ option.label }}</span>
            </li>
          </ul>
        </li>
        <li
          v-if="props.create_option && isQueryUnique"
          class="mt-1 border-t border-slate-200 h-1"
        ></li>
        <li
          v-if="props.create_option && isQueryUnique"
          @click="createOption(query)"
          class="px-4 py-2 cursor-pointer hover:bg-brand-50 text-sm font-medium text-brand-600 rounded-lg"
        >
          Create "{{ query }}"
        </li>
      </ul>
    </div>
    <span v-if="error" class="text-sm font-noraml text-radical-red-600">{{ error_message }}</span>
  </div>
</template>
