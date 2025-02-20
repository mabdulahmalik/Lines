<script setup lang="ts">
import { FwbButton, FwbTextarea } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { Encounter } from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';
const props = defineProps<{
  encounter?: Encounter;
}>();

const encountersStore = useEncountersStore();

const escalateEncounterModal = ref<InstanceType<typeof Modal>>();

const escalteReason = ref('');

function closeModal() {
  escalateEncounterModal.value?.setModalOpen(false);
}

function escalateEncounter() {
  if (props.encounter?.id) {
    encountersStore.escalateEncounter({
      encounterId: props.encounter.id,
      reason: escalteReason.value ? escalteReason.value : null,
    });
    escalateEncounterModal.value?.setModalOpen(false);
  }
}

const setModalOpen = (value: boolean): void => {
  escalateEncounterModal.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Escalate Encounter" :z_index="61" ref="escalateEncounterModal">
    <template #body>
      <div class="p-4 lg:p-6">
        <p class="text-xs text-slate-800 pb-2 italic">This will increase the visibility of this encounter by marking it as "STAT".</p>
        <fwb-textarea
          v-model="escalteReason"
          :rows="4"
          label="Escalation reason"
          placeholder="Write your reason..."
        />
        <span class="text-xs text-slate-500"> Max 64 characters </span>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="escalateEncounter" class="bg-primary-600 hover:bg-brand-600" pill>
          Escalate
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
