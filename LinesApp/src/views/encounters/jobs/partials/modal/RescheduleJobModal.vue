<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { FwbButton, FwbTextarea } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import VueTailwindDatepicker from 'vue-tailwind-datepicker';
import TimePicker from '@/components/form/TimePicker.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { Job } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { useFacilitiesStore } from '@/stores/data/facilities';
const props = defineProps<{
  job: Job;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
}>();

dayjs.extend(customParseFormat);

const jobsStore = useJobsStore();
const facilitiesStore = useFacilitiesStore();

const rescheduleJobModalRef = ref<InstanceType<typeof Modal>>();

onMounted(() => {
  date.value = '';
});

// Validations
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schema = yup.object({
  date: yup.string().required('Select a date'),
  time: yup.string().required('Select a time'),
  cancelReason: yup.string().required('Reason is required'),
});
const { handleSubmit, resetForm } = useForm({
  validationSchema: schema,
});

const isSubmitting = ref(false);
const { value: date, errorMessage: dateError } = useField<string>('date');
const { value: time, errorMessage: timeError } = useField<string>('time');
const { value: cancelReason, errorMessage: cancelReasonError } = useField<string>('cancelReason');

const formatter = ref({
  date: 'DD MMM YYYY',
  month: 'MMM',
});
const dDate = (dateToCheck: Date): boolean => {
  const today = new Date();
  return dateToCheck < today;
};

const parseDate = (dateStr: string): string => {
  return dayjs(dateStr, 'DD MMM YYYY').format('YYYY-MM-DD');
};
const parseTime = (timeStr: string): string => {
  return dayjs(timeStr, 'hh:mm A').format('HH:mm:ss');
};
const getFacilityTimezone = (facilityId: string): string => {
  return facilitiesStore.facilities.find((f) => f.id === facilityId)?.timeZone ?? '';
};

const handleReschedule = () => {
  isSubmitting.value = true;
  handleSubmit(async () => {
    if (props.job.id && props.job.location?.facilityId) {
      jobsStore.enactJobReschedule({
        id: props.job.id,
        facilityId: props.job.location?.facilityId,
        date: parseDate(date.value),
        time: time.value ? parseTime(time.value) : null,
        reason: cancelReason.value,
      });
      rescheduleJobModalRef.value?.setModalOpen(false);
      emit('close');
    }
  })();
};

function modalClosed() {
  resetForm();
  isSubmitting.value = false;
  date.value = '';
}

function closeModal() {
  rescheduleJobModalRef.value?.setModalOpen(false);
}
const setModalOpen = (value: boolean): void => {
  rescheduleJobModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal
    title="Reschedule Job"
    :z_index="71"
    ref="rescheduleJobModalRef"
    size="lg"
    @close="modalClosed"
  >
    <template #body>
      <div class="p-4 lg:p-6 pt-2 lg:pt-4 overflow-y-auto" style="max-height: calc(100vh - 300px)">
        <!-- Date and time -->
        <div class="flex flex-col gap-4">
          <div class="flex flex-wrap gap-4 justify-between">
            <vue-tailwind-datepicker
              v-model="date"
              as-single
              no-input
              :formatter="formatter"
              disable-in-range
              :disable-date="dDate"
              class="custom_dp mx-auto lg:mx-0 text-xs"
            />
            <div class="w-full lg:w-36">
              <TimePicker v-model="time" />
            </div>
          </div>

          <div
            v-if="date || time"
            class="py-2 px-4 w-full bg-brand-50 rounded-lg text-barnd-600 text-xs font-bold text-center"
          >
            {{ date }}<span v-if="date && time">, &nbsp;</span> {{ time }}
            <span v-if="date && time && props.job.location?.facilityId"
              >({{ getFacilityTimezone(props.job.location.facilityId) }})</span
            >
          </div>
        </div>
        <div v-if="(dateError || timeError) && isSubmitting" class="text-sm text-radical-red-600">
          {{ dateError || timeError }}
        </div>
        <div class="pt-3">
          <fwb-textarea
            v-model="cancelReason"
            :rows="3"
            label="Add reason"
            placeholder="Write your reason..."
            :class="cancelReasonError ? inputErrorClasses : ''"
          />
          <span v-if="cancelReasonError" class="text-sm text-radical-red-600">{{
            cancelReasonError
          }}</span>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="handleReschedule" class="bg-primary-600 hover:bg-brand-600" pill>
          Reschedule
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>

<style scoped>
:deep(.custom_dp > .shadow-sm) {
  padding: 0;
  border: 0;
  box-shadow: none;
}
:deep(.custom_dp .relative.flex.flex-wrap.w-full) {
  padding-top: 0;
  padding-bottom: 0;
}
:deep(.custom_dp .relative.w-full.lg\:w-80) {
  width: 16rem;
}
:deep(.custom_dp .flex.justify-between.items-center.px-2) {
  border-bottom: solid 1px #e2e8f0;
}
</style>
