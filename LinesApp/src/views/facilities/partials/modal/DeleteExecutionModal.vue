<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { RoutineAssignmentPrm } from '@/api/__generated__/graphql';

// Props
const props = defineProps<{ exec: RoutineAssignmentPrm }>();

// Emits
const emit = defineEmits(['close', 'deletedExec']);

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const handleModalClosed = () => {
  emit('close');
};

const confirmDelete = () => {
  emit('deletedExec', props.exec);
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
  <Modal
    ref="modalRef"
    max_width="xl"
    set_margins
    @close="handleModalClosed"
  >
    <!-- Header -->
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">
        Delete Assignment
      </h3>
    </template>

    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Deleting this routine assignment removes it from this facility only, without affecting the routine itself or its use in other facilities.
          <br /><br />
          Automatic scheduling of certain Jobs will stop.
          <br /><br />
          Are you sure you want to delete the <span class="text-purple-800 px-0.5">{{ props.exec?.name }}</span> assignment?
        </div>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>
          No, leave it
        </fwb-button>
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
