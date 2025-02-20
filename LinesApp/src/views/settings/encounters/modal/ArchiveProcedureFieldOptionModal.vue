<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ProcedureFieldOption } from '@/api/__generated__/graphql';

// Props
const props = defineProps<{ procedureFieldOption: ProcedureFieldOption }>();

// Emits
const emit = defineEmits(['archivedOption']);

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const handleModalClosed = () => {
  closeModal();
};

const confirmArchive = () => {
  emit('archivedOption', props.procedureFieldOption);
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
  <Modal ref="modalRef" max_width="xl" set_margins @close="handleModalClosed" title="Archive List Value">
    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Procedure field list value <span class="text-purple-800 px-0.5">{{ props.procedureFieldOption?.text }}</span> is being
          referenced by {{ props.procedureFieldOption?.references }} Job(s), Encounter(s), or Routine(s) and
          <strong>cannot be deleted</strong>, only archived. <br /><br />
          Archiving this procedure field list value will prevent it from being used in the future, but will not affect
          any existing data and will still show in reports.
          <br /><br />
          Would you like to archive it instead?
        </div>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>No, leave it</fwb-button>
        <fwb-button
          @click="confirmArchive"
          class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
          pill
        >
          Yes, archive it
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
