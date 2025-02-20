<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { Purpose } from '@/api/__generated__/graphql';
import { usePurposesStore } from '@/stores/data/settings/purposes';

// Props
const props = defineProps<{ purpose: Purpose }>();

// Stores
const purposesStore = usePurposesStore();

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const handleModalClosed = () => {
  closeModal();
};

const confirmRestore = () => {
  purposesStore.unarchivePurpose({ id: props.purpose.id });
  closeModal();
};

// Expose API
defineExpose({
  setModalOpen: (value: boolean) => {
    modalRef.value?.setModalOpen(value);
  },
});
</script>

<template>
  <Modal ref="modalRef" max_width="xl" set_margins @close="handleModalClosed" title="Restore Purpose">
    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Restoring a purpose will make it accessible again, restoring it to its original state.
          <br /><br />
          Would you like to restore
          <span class="text-purple-800 px-0.5">{{ props.purpose.name }}</span
          >?
        </div>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>No, leave it</fwb-button>
        <fwb-button
          @click="confirmRestore"
          class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
          pill
        >
          Yes, restore it
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
