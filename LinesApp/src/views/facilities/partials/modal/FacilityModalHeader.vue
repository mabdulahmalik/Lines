<script setup lang="ts">
import { FwbButton, FwbInput } from 'flowbite-vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import {
  IconExpand01,
  IconDotHorizontal,
  IconArrowLeft,
  IconTrash01,
  IconEdit03,
  IconShrink,
  IconX,
  IconPlayCircle,
} from '@/components/icons/index';
import { ref, onMounted, computed } from 'vue';
import Modal from '@/components/modal/Modal.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useModalStore } from '@/stores/modal';
import { Facility } from '@/api/__generated__/graphql';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useFacilitiesStore } from '@/stores/data/facilities';
const modalStore = useModalStore();
const props = defineProps<{
  facility: Facility;
}>();
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const fullWidth = ref(false);
const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'width', val: string): void;
  (e: 'facilityName', val: string): void;
}>();

const facilitiesStore = useFacilitiesStore();
const facilityTypesStore = useFacilityTypesStore();
const facilityTypes = computed(() => facilityTypesStore.facilityTypes);

const closeModal = () => {
  emit('close');
};
const setModalWidth = (val: string) => {
  emit('width', val);
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isFacilitiesModalExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isFacilitiesModalExpended = false;
  }
};

// Edit Facility Name

// Validation
const schemaEditFacilityName = yup.object({
  facilityNameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSaveFacilityName } = useForm({
  validationSchema: schemaEditFacilityName,
});

const { value: facilityNameEdit, errorMessage: editFacilityNameError } =
  useField<string>('facilityNameEdit');

// Facility Name
const facilityName = ref(props.facility.name);
const isEditFacilityName = ref(false);
onMounted(() => {
  facilityName.value = props.facility.name;
});

const handleClickFacilityNameEditIcon = () => {
  facilityNameEdit.value = facilityName.value!;
  isEditFacilityName.value = true;
};

const cancelEditFacilityName = () => {
  isEditFacilityName.value = false;
};

function saveEditfacilityName() {
  handleSaveFacilityName(async () => {
    facilityName.value = facilityNameEdit.value;
    isEditFacilityName.value = false;
    emit('facilityName', facilityName.value);
  })();
}

// Get FacilityType Name
const facilityTypeName = computed(() => {
  const matchingFacility = facilityTypes.value?.find(
    (val) => facilitiesStore.selectedFacility.typeId === val.id
  );
  return matchingFacility?.name;
});

// Delete Facility Modal

const deleteFacilityModalRef = ref<InstanceType<typeof Modal>>();
const closeDeleteFacilityModal = () => {
  deleteFacilityModalRef.value?.setModalOpen(false);
};

// Validation

const schemaDeleteFacility = yup.object({
  facilityNameForDelete: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitDeleteFacility } = useForm({
  validationSchema: schemaDeleteFacility,
});
const {
  value: facilityNameForDelete,
  errorMessage: facilityNameError,
  resetField: resetFacilityName,
} = useField<string>('facilityNameForDelete');

function clickDeleteFacility() {
  if (props.facility.referenceCount && props.facility.referenceCount > 0) {
    archiveFacilityModalRef?.value?.setModalOpen(true);
  } else {
    deleteFacilityModalRef?.value?.setModalOpen(true);
  }
}

function handleDeleteFacility() {
  handleSubmitDeleteFacility(async () => {
    facilitiesStore.deleteFacility({ id: props.facility.id });
    closeDeleteFacilityModal();
    resetFacilityName();
    emit('close');
  })();
}

// Archive Facility Modal
const archiveFacilityModalRef = ref<InstanceType<typeof Modal>>();
const closeArchiveFacilityModal = () => {
  archiveFacilityModalRef.value?.setModalOpen(false);
};
function handleArchiveFacility() {
  facilitiesStore.archiveFacility({ id: props.facility.id });
  closeArchiveFacilityModal();
  emit('close');
}

// Restore Facility Modal

const restoreFacilityModalRef = ref<InstanceType<typeof Modal>>();
const closeRestoreFacilityModal = () => {
  restoreFacilityModalRef.value?.setModalOpen(false);
};
function handleRestoreFacility() {
  facilitiesStore.unarchiveFacility({ id: props.facility.id });
  closeRestoreFacilityModal();
}
</script>

<template>
  <div>
    <!-- Mobile status badge -->    
    <div
      class="lg:hidden py-[9px] px-2.5 text-xs leading-[18px] font-medium flex justify-center items-center text-center capitalize bg-green-100 text-green-600"
    >
      {{ facilityTypeName }}
    </div>
    <div v-if="props.facility.archived"
      class="lg:hidden py-[9px] px-2.5 text-xs leading-[18px] font-medium flex justify-center items-center text-center capitalize bg-[#FCE8F3] text-[#99154B]"
    >
      Archived
    </div>     

    <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
      <div class="flex items-center">
        <!-- Mobile close button -->
        <fwb-button
          @click="closeModal"
          color="light"
          pill
          square
          size="lg"
          class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
        >
          <IconArrowLeft width="24" height="24" class="text-[#334155]" />
          <span class="sr-only">Close modal</span>
        </fwb-button>

        <div v-if="isEditFacilityName" class="flex items-center gap-3">
          <fwb-input
            size="sm"
            v-model.trim="facilityNameEdit"
            placeholder="Write text here"
            class="sm:w-[295px]"
            :class="editFacilityNameError ? inputErrorClasses : ''"
          />
          <div class="flex items-center gap-3">
            <fwb-button @click="cancelEditFacilityName" size="sm" color="light" pill>
              Cancel</fwb-button
            >
            <fwb-button
              @click="saveEditfacilityName"
              size="sm"
              class="bg-primary-600 hover:bg-brand-600"
              pill
              >Confirm</fwb-button
            >
          </div>
        </div>
        <div v-else class="flex items-center gap-3">
          <h2 class="text-lg lg:text-2xl font-semibold">
            {{ facilityName }}
          </h2>
          <fwb-button
           v-if="!facilitiesStore.selectedFacility.archived"
            @click.stop="handleClickFacilityNameEditIcon"
            color="light"
            pill
            square
            class="border-transparent"
          >
            <IconEdit03 />
          </fwb-button>
        </div>
      </div>

      <div class="lg:hidden">
        <!-- Mobile More dropdown  -->
        <Dropdown align-to-end close-inside class="rounded-lg">
          <template #trigger>
            <fwb-button color="light" pill square>
              <IconDotHorizontal />
            </fwb-button>
          </template>
          <DropdownMenu>
            <DropdownItem @click="clickDeleteFacility" class="!text-radical-red-700">
              <IconTrash01 class="mr-2" />
              Delete Facility
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>
      </div>

      <!-- Icon Buttons Desktop only -->
      <div class="hidden lg:flex items-center gap-2 lg:gap-4">
        <!-- status -->
        <span v-if="props.facility.archived"
          class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 capitalize bg-[#FCE8F3] text-[#99154B]"
          >Archived</span
        >         
        <span
          class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 capitalize bg-green-100 text-green-600"
          >{{ facilityTypeName }}</span
        >

        <!-- dropdown -->
        <Dropdown align-to-end close-inside class="rounded-lg">
          <template #trigger>
            <fwb-button color="light" pill square>
              <IconDotHorizontal />
            </fwb-button>
          </template>
          <DropdownMenu>
            <DropdownItem
              v-if="props.facility.archived"
              @click="restoreFacilityModalRef?.setModalOpen(true)"
              class=""
            >
              <IconPlayCircle class="mr-2" />
              Restore Facility
            </DropdownItem>
            <DropdownItem v-else @click="clickDeleteFacility" class="!text-radical-red-700">
              <IconTrash01 class="mr-2" />
              Delete Facility
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>

        <!-- Expand -->
        <fwb-button v-if="!fullWidth" @click="setModalWidth('full')" color="light" pill square>
          <IconExpand01 />
        </fwb-button>
        <!-- Shrink -->
        <fwb-button v-if="fullWidth" @click="setModalWidth('5xl')" color="light" pill square>
          <IconShrink />
        </fwb-button>
      </div>
    </div>

    <!-- Delete Facility Modal -->
    <teleport to="body">
      <Modal ref="deleteFacilityModalRef" title="Delete Facility" size="lg" :z_index="55">
        <template #body>
          <div class="p-4 lg:p-6">
            <div class="flex flex-col gap-4">
              <div class="text-sm font-medium">
                Deleting this facility will permanently remove it from your Account.
              </div>
              <div class="text-sm font-bold">I understand that</div>
              <div class="text-radical-red-500 flex flex-col gap-2 text-xs font-medium">
                <div class="flex sm:items-center gap-2">
                  <IconX class="flex-shrink-0" /> This Facility will no longer be accessible
                </div>
                <div class="flex sm:items-center gap-2">
                  <IconX class="flex-shrink-0" /> All data associated with this Facility (Rooms,
                  etc) will be removed
                </div>
                <div class="flex sm:items-center gap-2">
                  <IconX class="flex-shrink-0" /> This action cannot be undone
                </div>
              </div>
              <div>
                <label class="mb-2 block text-sm font-medium"
                  >Confirm by typing
                  <span class="text-purple-800 px-0.5">{{ props.facility.name }}</span>
                  below.</label
                >
                <fwb-input
                  v-model.trim="facilityNameForDelete"
                  :class="facilityNameError ? inputErrorClasses : ''"
                  placeholder="Facility Name in here"
                />
                <span v-if="facilityNameError" class="text-sm text-radical-red-600">{{
                  facilityNameError
                }}</span>
              </div>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="closeDeleteFacilityModal" color="light" pill> Cancel</fwb-button>
            <fwb-button
              @click.prevent="handleDeleteFacility"
              class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
              pill
              :disabled="facilityNameForDelete !== props.facility.name"
            >
              Delete Facility
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>

    <!-- Archive Facility Modal -->
    <teleport to="body">
      <Modal ref="archiveFacilityModalRef" title="Delete Facility" size="lg" :z_index="55">
        <template #body>
          <div class="p-4 lg:p-6">
            <div class="flex flex-col gap-4">
              <div class="text-sm font-medium">
                Facility <span class="text-purple-800 px-0.5">{{ props.facility.name }}</span> ie
                being referenced by {{ props.facility.referenceCount }} Job(s), Encounter(s), or
                Line(s) and <strong>cannot be deleted</strong>, only archived. <br /><br />
                Archiving this facility will prevent it from being used in the future, but will not
                affect any existing data and will still show in reports.
                <br /><br />
                Would you like to archive it instead?
              </div>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="closeArchiveFacilityModal" color="light" pill>No, leave it</fwb-button>
            <fwb-button
              @click.prevent="handleArchiveFacility"
              class="bg-primary-600 hover:bg-brand-600 focus:ring-0"
              pill
            >
              Yes, archive it
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>

    <!-- Restore Facility Modal -->
    <teleport to="body">
      <Modal ref="restoreFacilityModalRef" title="Restore Facility" size="lg" :z_index="55">
        <template #body>
          <div class="p-4 lg:p-6">
            <div class="flex flex-col gap-4">
              <div class="text-sm font-medium">
                Restoring a facility will make it accessible again, restoring it to its original state.
                <br /><br />
                Would you like to restore 
                <span class="text-purple-800 px-0.5">{{ props.facility.name }}</span>?
              </div>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="closeRestoreFacilityModal" color="light" pill>No, leave it</fwb-button>
            <fwb-button
              @click.prevent="handleRestoreFacility"
              class="bg-primary-600 hover:bg-brand-600 focus:ring-0"
              pill
            >
              Yes, restore it
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
  </div>
</template>
