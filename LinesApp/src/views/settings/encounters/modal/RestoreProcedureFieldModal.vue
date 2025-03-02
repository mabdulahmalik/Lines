<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ProcedureField } from '@/api/__generated__/graphql';

// Props
const props = defineProps<{ procedureField: ProcedureField }>();

// Emits
const emit = defineEmits(['restoredField']);

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const confirmRestore = () => {
  emit('restoredField', props.procedureField);
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
  <Modal ref="modalRef" max_width="xl" set_margins title="Restore Procedure Field">
    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Restoring a procedure field will make it accessible again, restoring it to its original state.
          <br /><br />
          Would you like to restore
          <span class="text-purple-800 px-0.5">{{ props.procedureField.name }}</span
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
