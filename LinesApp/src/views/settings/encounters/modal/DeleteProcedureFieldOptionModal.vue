<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ProcedureFieldOption } from '@/api/__generated__/graphql';

// Props
const props = defineProps<{ procedureFieldOption: ProcedureFieldOption }>();

// Emits
const emit = defineEmits(['deletedOption']);

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const confirmDelete = () => {
  emit('deletedOption', props.procedureFieldOption);
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
  <Modal ref="modalRef" max_width="xl" set_margins title="Delete List Value">
    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Are you sure you want to delete the
          <span class="text-purple-800 px-0.5">{{ props.procedureFieldOption?.text }}</span> value?
        </div>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>No, leave it</fwb-button>
        <fwb-button
          @click="confirmDelete"
          class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
          pill
        >
          Yes, delete it
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
