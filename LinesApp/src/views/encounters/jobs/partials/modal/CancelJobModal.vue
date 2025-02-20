<script setup lang="ts">
import { FwbButton, FwbTextarea } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { Job } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
const props = defineProps<{
  job: Job;
}>();

const jobsStore = useJobsStore();

const cancelJobModalRef = ref<InstanceType<typeof Modal>>();

const cancelReason = ref('');

function closeModal() {
  cancelJobModalRef.value?.setModalOpen(false);
}

function cancelJob() {
  if (props.job.id) {
    jobsStore.cancelJob({
      id: props.job.id,
      reason: cancelReason.value ? cancelReason.value : null,
    });
    cancelJobModalRef.value?.setModalOpen(false);
  }
}

const setModalOpen = (value: boolean): void => {
  cancelJobModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Cancel Job" :z_index="61" ref="cancelJobModalRef">
    <template #body>
      <div class="p-4 lg:p-6">
        <p class="text-xs text-slate-800 pb-2 italic">This immediately ends the Job, and any existing Encounters, but does not remove it from the Job history.</p>
        <fwb-textarea
          v-model="cancelReason"
          :rows="3"
          label="Reason for Cancellation"
          placeholder="Write your reason..."
        />
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="cancelJob" class="bg-primary-600 hover:bg-brand-600" pill>
            Confirm
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
