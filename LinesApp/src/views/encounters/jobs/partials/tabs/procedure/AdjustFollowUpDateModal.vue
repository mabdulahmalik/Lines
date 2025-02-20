<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import GlobalModal from '@/components/modal/GlobalModal.vue';
import { useModalStore } from '@/stores/modal';
import VueTailwindDatepicker from 'vue-tailwind-datepicker';
import { ref } from 'vue';
import InputNumberCounter from '@/components/form/InputNumberCounter.vue';
import OutlineIcon from '@/components/styled-icon/OutlineIcon.vue';
import { IconSpacingWidth01 } from '@/components/icons';

const modalStore = useModalStore();

const followUpDate = ref('');
const formatter = ref({
  date: 'DD MMM YYYY',
  month: 'MMM',
});

function closeModal() {
  modalStore.closeGlobalModal();
}

function handleConfirm() {
  modalStore.closeGlobalModal();
}
</script>

<template>
  <GlobalModal title="&nbsp;" :z_index="101">
    <template #body>
      <div class="p-4 lg:p-6">
        <div class="flex flex-col items-center gap-2">
          <OutlineIcon size="xl" type="error">
            <IconSpacingWidth01></IconSpacingWidth01>
          </OutlineIcon>
          <div class="text-lg font-semibold text-slate-900">Adjust date of procedure follow up</div>
        </div>
        <div class="flex items-center justify-between py-4">
          <div class="text-sm font-medium flex-1">Days</div>
          <InputNumberCounter :model-value="7" class="max-w-40"></InputNumberCounter>
        </div>
        <hr class="border-slate-300" />
        <div class="custom_dp pt-4">
          <vue-tailwind-datepicker
            v-model="followUpDate"
            as-single
            no-input
            :formatter="formatter"
            class="custom_dp mx-auto lg:mx-0"
          />
          <div
            v-if="followUpDate"
            class="mt-2 py-2 px-4 w-full bg-brand-50 rounded-lg text-barnd-600 text-xs font-bold text-center"
          >
            {{ followUpDate }}
          </div>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="handleConfirm" class="bg-primary-600 hover:bg-brand-600" pill>
          Confirm
        </fwb-button>
      </div>
    </template>
  </GlobalModal>
</template>

<style scoped>
:deep(.custom_dp > .shadow-sm) {
  padding: 0;
  border: 0;
  box-shadow: none;
}
:deep(.custom_dp .relative.flex.flex-wrap.w-full) {
  padding: 0;
}
</style>
