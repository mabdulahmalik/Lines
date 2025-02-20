<script setup lang="ts">
import { FwbButton, FwbTextarea } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { Encounter } from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';

const props = defineProps<{
  encounter: Encounter;
}>();

const encounterStore = useEncountersStore();

const modalRef = ref<InstanceType<typeof Modal>>();

const reason = ref('');

function closeModal() {
  modalRef.value?.setModalOpen(false);
}

function requestAssistance() {
  if (props.encounter.id) {
    encounterStore.requestAssistanceForEncounter({
      encounterId: props.encounter.id,
      reason: reason.value ? reason.value : null,
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
  <Modal title="Need Help with this Patient?" :z_index="61" ref="modalRef">
    <template #body>
      <div class="p-4 lg:p-6">
        <fwb-textarea
          v-model="reason"
          :rows="4"
          label="Reason for help (optional)"
          placeholder="Provide context of what you're currently doing and what you need help with."
        />
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>No, I got it</fwb-button>
        <fwb-button @click="requestAssistance" class="bg-primary-600 hover:bg-brand-600" pill>
            Yes, alert my colleagues
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
