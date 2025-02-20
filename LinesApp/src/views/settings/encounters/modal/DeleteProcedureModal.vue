<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton, FwbInput } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { Procedure } from '@/api/__generated__/graphql';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { IconX } from '@/components/icons';

// Props
const props = defineProps<{ procedure: Procedure }>();

// Emits
const emit = defineEmits(['close']);

// Stores
const proceduresStore = useProceduresStore();

// State
const procedureNameConfirmation = ref('');

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const handleModalClosed = () => {
  closeModal();
};

const confirmDelete = () => {
  proceduresStore.deleteProcedure({ id: props.procedure.id });
  closeModal();
  emit('close');
};

// Expose API
defineExpose({
  setModalOpen: (value: boolean) => {
    modalRef.value?.setModalOpen(value);
  },
});
</script>

<template>
  <Modal ref="modalRef" size="lg" @close="handleModalClosed" title="Delete Procedure">
    <!-- Body -->
    <template #body>
      <div class="p-4 lg:p-6">
        <div class="flex flex-col gap-4">
          <div class="text-sm font-medium">
            Deleting this procedure will permanently remove it from your account.
          </div>
          <div class="text-sm font-bold">I understand that</div>
          <div class="text-radical-red-500 flex flex-col gap-2 text-xs font-medium">
            <div class="flex sm:items-center gap-2">
              <IconX class="flex-shrink-0" /> This Procedure will no longer be accessible
            </div>
            <div class="flex sm:items-center gap-2">
              <IconX class="flex-shrink-0" /> This action cannot be undone
            </div>
          </div>
          <div>
            <label class="mb-2 block text-sm font-medium"
              >Confirm by typing
              <span class="text-purple-800 px-0.5">{{ props.procedure.name }}</span>
              below.</label
            >
            <fwb-input
              v-model.trim="procedureNameConfirmation"
              placeholder="Procedure Name in here"
            />
          </div>
        </div>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>Cancel</fwb-button>
        <fwb-button
          @click="confirmDelete"
          class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
          pill
          :disabled="procedureNameConfirmation !== props.procedure.name"
        >
          Delete Procedure
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
