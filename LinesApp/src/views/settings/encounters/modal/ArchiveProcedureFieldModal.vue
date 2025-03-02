<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ProcedureField } from '@/api/__generated__/graphql';

// Props
const props = defineProps<{ procedureField: ProcedureField }>();

// Emits
const emit = defineEmits(['archivedField']);

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const confirmArchive = () => {
  emit('archivedField', props.procedureField);
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
  <Modal ref="modalRef" max_width="xl" set_margins title="Archive Procedure Field">
    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Procedure field <span class="text-purple-800 px-0.5">{{ props.procedureField?.name }}</span> is being
          referenced by {{ props.procedureField?.references }} Job(s), Encounter(s), or Routine(s) and
          <strong>cannot be deleted</strong>, only archived. <br /><br />
          Archiving this procedure field will prevent it from being used in the future, but will not affect
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
