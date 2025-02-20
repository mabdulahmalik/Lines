<script setup lang="ts">
import { FwbAvatar } from 'flowbite-vue';
import { useProviderStore } from '@/stores/provider';
import { storeToRefs } from 'pinia';
import { ProviderType } from '@/types/provider';
import { ref } from 'vue';

const { providers, activeProvider } = storeToRefs(useProviderStore());

const checked = ref({ ...activeProvider.value });

function changeProvider(id: string) {
  const provider = providers.value.find((provider) => provider.id === id);
  if (provider) {
    checked.value = provider;
    emit('switch', provider);
  }
}
const emit = defineEmits<{
  (e: 'switch', provider: ProviderType): void;
}>();

</script>

<template>
  <div class="flex flex-col gap-3">
    <div
      @click="changeProvider(provider.id)"
      v-for="(provider, index) in providers"
      :key="index"
      class="flex items-center justify-between rounded-lg font-medium text-sm text-slate-900 py-3 px-3 duration-300 ease-in-out hover:bg-brand-50 cursor-pointer"
      :class="{ 'bg-brand-50': provider.id === checked.id }"
    >
      <div class="flex items-center">
        <fwb-avatar img="" rounded class="mr-4" />
        <div class="">
          <div class="font-semibold text-sm">{{ provider.name }}</div>
          <span class="text-xs text-slate-500">
            {{ provider.clinicians }}
          </span>
        </div>
      </div>
      <div v-if="provider.id === checked.id" class="flex">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="16"
          height="16"
          viewBox="0 0 16 16"
          fill="none"
        >
          <path
            d="M13.3332 4L5.99984 11.3333L2.6665 8"
            stroke="#6A5ACD"
            stroke-width="1.5"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
        </svg>
      </div>
    </div>
  </div>
</template>
