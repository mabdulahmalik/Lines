<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { Procedure } from '@/api/__generated__/graphql';
import { useProceduresStore } from '@/stores/data/settings/procedures';

// Props
const props = defineProps<{ procedure: Procedure }>();

// Emits
const emit = defineEmits(['close']);

// Stores
const proceduresStore = useProceduresStore();

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const confirmArchive = () => {
  proceduresStore.archiveProcedure({ id: props.procedure.id });
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
  <Modal ref="modalRef" max_width="xl" set_margins title="Archive Procedure">
    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Procedure <span class="text-purple-800 px-0.5">{{ props.procedure?.name }}</span> is being
          referenced by {{ props.procedure?.references }} Job(s), Encounter(s), or Routine(s) and
          <strong>cannot be deleted</strong>, only archived. <br /><br />
          Archiving this procedure will prevent it from being used in the future, but will not affect
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
