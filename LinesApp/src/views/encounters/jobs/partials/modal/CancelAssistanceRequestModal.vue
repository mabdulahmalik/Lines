<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { Encounter } from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';

const props = defineProps<{
  encounter: Encounter;
}>();

const encounterStore = useEncountersStore();

const modalRef = ref<InstanceType<typeof Modal>>();

function closeModal() {
  modalRef.value?.setModalOpen(false);
}

function cancelRequest() {
  if (props.encounter.id) {
    encounterStore.cancelAssistanceRequestForEncounter({
      encounterId: props.encounter.id
    });
    modalRef.value?.setModalOpen(false);
  }
}

const setModalOpen = (value: boolean): void => {
  modalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Cancel Assistance Request" :z_index="61" ref="modalRef">
    <template #body>
      <div class="p-4 lg:p-6">
        <span class="text-sm font-medium">Are you sure to cancel the assistance request? </span>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="cancelRequest" class="bg-primary-600 hover:bg-brand-600" pill>
            Yes
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
