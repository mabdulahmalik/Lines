<script setup lang="ts">
import { FwbButton, FwbTextarea } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { useEncountersStore } from '@/stores/data/encounters';
import { Encounter } from '@/api/__generated__/graphql';
const props = defineProps<{
  encounter?: Encounter;
}>();

const encountersStore = useEncountersStore();

const putOnHoldModalRef = ref<InstanceType<typeof Modal>>();

const onHoldReason = ref('');

function closeModal() {
  putOnHoldModalRef.value?.setModalOpen(false);
}

function putOnHold() {
  if (props.encounter?.id) {
    encountersStore.placeEncounterOnHold({
      encounterId: props.encounter?.id,
      reason: onHoldReason.value ? onHoldReason.value : null,
    });
    putOnHoldModalRef.value?.setModalOpen(false);
  }
}

const setModalOpen = (value: boolean): void => {
  putOnHoldModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Put On Hold" :z_index="61" ref="putOnHoldModalRef">
    <template #body>
      <div class="p-4 lg:p-6">
        <fwb-textarea
          v-model="onHoldReason"
          :rows="4"
          label="On Hold reason"
          placeholder="Write your reason..."
        />
        <span class="text-xs text-slate-500"> Max 64 characters </span>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="putOnHold" class="bg-primary-600 hover:bg-brand-600" pill>
          Put On Hold
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
