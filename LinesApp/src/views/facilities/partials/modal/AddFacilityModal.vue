<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import { FwbButton, FwbSelect, FwbInput } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { CreateFacilityCmd } from '@/api/__generated__/graphql';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useStatesStore } from '@/stores/data/location/states';
import { useTimezonesStore } from '@/stores/data/location/timezones';

const facilitiesStore = useFacilitiesStore();
const statesStore = useStatesStore();
const timezonesStore = useTimezonesStore();

const facilityTypesStore = useFacilityTypesStore();
const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const addFacilityModalRef = ref<InstanceType<typeof ModalDrawer>>();
function closeAddFacilityModal() {
  addFacilityModalRef.value?.setModalOpen(false);
}

const setModalOpen = (value: boolean): void => {
  addFacilityModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});

// type options
const facilityTypeOptions = computed(() =>
  facilityTypes.value
  .filter((r) => r.isActive)
  .map((r) => ({
    value: r.id,
    name: r.name!,
  }))
);

// states & timezone options
interface Option {
  value: string;
  name: string;
}
const stateOptions = ref<Option[]>([]);
const timeZoneOptions = ref<Option[]>([]);
watch(
  () => statesStore.states,
  (newStates) => {
    stateOptions.value = newStates.map((state) => ({
      value: state.code,
      name: state.name,
    }));
  },
  { immediate: true }
);

watch(
  () => timezonesStore.timeZones,
  (newStates) => {
    timeZoneOptions.value = newStates.map((timeZone) => ({
      value: timeZone,
      name: timeZone,
    }));
  },
  { immediate: true }
);

// Validation
const schemaAddFacility = yup.object({
  facilityType: yup.string().required('This is a required field'),
  facilityName: yup.string().required('This is a required field').trim(),
  facilityStreet: yup.string().required('This is a required field').trim(),
  facilityCity: yup.string().required('This is a required field').trim(),
  facilityState: yup.string().required('This is a required field'),
  facilityZipCode: yup
    .string()
    .required('This is a required field')
    .matches(/^\d{5}$/, 'Zip code must be at 5 digits'),
  facilityTimeZone: yup.string().required('This is a required field'),
});

const { handleSubmit: handleSubmitAddFacility, errors } = useForm({
  validationSchema: schemaAddFacility,
});

const {
  value: facilityType,
  errorMessage: facilityTypeError,
  resetField: resetType,
} = useField<string>('facilityType');
const {
  value: facilityName,
  errorMessage: facilityNameError,
  resetField: resetName,
} = useField<string>('facilityName');
const {
  value: facilityStreet,
  errorMessage: facilityStreetError,
  resetField: resetStreet,
} = useField<string>('facilityStreet');
const {
  value: facilityCity,
  errorMessage: facilityCityError,
  resetField: resetCity,
} = useField<string>('facilityCity');
const {
  value: facilityState,
  errorMessage: facilityStateError,
  resetField: resetState,
} = useField<string>('facilityState');
const {
  value: facilityZipCode,
  errorMessage: facilityZipCodeError,
  resetField: resetZipCode,
} = useField<string>('facilityZipCode');
const {
  value: facilityTimeZone,
  errorMessage: facilityTimeZoneError,
  resetField: resetTimeZone,
} = useField<string>('facilityTimeZone');

function handleAddFacility() {
  handleSubmitAddFacility(async () => {
    const newFacility: CreateFacilityCmd = {
      typeId: facilityType.value,
      name: facilityName.value,
      address: {
        street: facilityStreet.value,
        city: facilityCity.value,
        state: facilityState.value,
        zipCode: facilityZipCode.value,
      },
      timeZone: facilityTimeZone.value,
      requiredData: [],
    };
    facilitiesStore.createFacility(newFacility);
    resetType(),
      resetName(),
      resetStreet(),
      resetCity(),
      resetState(),
      resetZipCode(),
      resetTimeZone();
    closeAddFacilityModal();
  })();
}
</script>

<template>
  <ModalDrawer ref="addFacilityModalRef" close_outside max_width="xl" title="Add Facility">
    <template #body>
      <!-- body -->
      <div class="flex flex-col gap-4 p-4 lg:p-8">
        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Type</label>
          <fwb-select
            v-model="facilityType"
            :options="facilityTypeOptions"
            :class="facilityTypeError ? inputErrorClasses : ''"
            placeholder="Select"
          />
          <span v-if="facilityTypeError" class="text-sm text-radical-red-600">{{
            facilityTypeError
          }}</span>
        </div>

        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Name</label>
          <fwb-input v-model="facilityName" :class="facilityNameError ? inputErrorClasses : ''" />
          <span v-if="facilityNameError" class="text-sm text-radical-red-600">{{
            facilityNameError
          }}</span>
        </div>
        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Street</label>
          <fwb-input
            v-model="facilityStreet"
            :class="facilityStreetError ? inputErrorClasses : ''"
          />
          <span v-if="facilityStreetError" class="text-sm text-radical-red-600">{{
            facilityStreetError
          }}</span>
        </div>
        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">City</label>
          <fwb-input v-model="facilityCity" :class="facilityCityError ? inputErrorClasses : ''" />
          <span v-if="facilityCityError" class="text-sm text-radical-red-600">{{
            facilityCityError
          }}</span>
        </div>

        <div class="flex sm:flex-row flex-col gap-4">
          <div class="flex-1">
            <label class="mb-1 block text-sm font-medium text-slate-900">State</label>
            <fwb-select
              v-model="facilityState"
              :options="stateOptions"
              placeholder="Select"
              :class="facilityStateError ? inputErrorClasses : ''"
            />
            <span v-if="facilityStateError" class="text-sm text-radical-red-600">{{
              facilityStateError
            }}</span>
          </div>
          <div>
            <label class="mb-1 block text-sm font-medium text-slate-900">Zip Code</label>
            <fwb-input
              v-model="facilityZipCode"
              :class="facilityZipCodeError ? inputErrorClasses : ''"
            />
            <span v-if="facilityZipCodeError" class="text-sm text-radical-red-600">{{
              facilityZipCodeError
            }}</span>
          </div>
        </div>

        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Timezone</label>
          <fwb-select
            v-model="facilityTimeZone"
            :options="timeZoneOptions"
            placeholder="Select"
            :class="facilityTimeZoneError ? inputErrorClasses : ''"
          />
          <span v-if="facilityTimeZoneError" class="text-sm text-radical-red-600">{{
            facilityTimeZoneError
          }}</span>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="px-6 py-4 flex items-center justify-end w-full">
        <div class="flex gap-6 sm:w-auto w-full items-center">
          <span v-if="Object.keys(errors).length" class="text-sm font-normal text-radical-red-600"
            >{{ Object.keys(errors).length }} errors</span
          >
          <fwb-button @click="closeAddFacilityModal" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click.prevent="handleAddFacility"
            class="bg-primary-600 hover:bg-brand-600 flex-1"
            pill
          >
            Save
          </fwb-button>
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>
