<script setup lang="ts">
import Tabs from '@/components/tabs/Tabs.vue';
import { ref, computed, onMounted, onUnmounted, watch, nextTick } from 'vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import Modal from '@/components/modal/Modal.vue';
import { FwbInput, FwbButton, FwbRadio } from 'flowbite-vue';
import { IconTrash01, IconDotsReorder, IconArrowNarrowDown } from '@/components/icons/index';
import { BaseTable, THead, TR, TD, TH, TBody } from '@/components/table/index';
import draggable from 'vuedraggable';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import {
  CreatePurposeCmd,
  Procedure,
  ProcedureType,
  Purpose,
} from '@/api/__generated__/graphql';
import CreateProcedureModal from './modal/CreateProcedureModal.vue';
import ViewProcedureModal from './modal/ViewProcedureModal.vue';
import { useTabStore } from '@/stores/tab';
import ArchivePurposeModal from './modal/ArchivePurposeModal.vue';
import DeletePurposeModal from './modal/DeletePurposeModal.vue';
import RestorePurposeModal from './modal/RestorePurposeModal.vue';
import ButtonGroupRadio from '@/components/form/ButtonGroupRadio.vue';
import ButtonRadio from '@/components/form/ButtonRadio.vue';

const proceduresStore = useProceduresStore();
const purposesStore = usePurposesStore();
const breadcrumbStore = useBreadcrumbStore();
const tabStore = useTabStore();
onMounted(() => {
  tabStore.settingsEncountersActiveTab = 0;
});

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Settings', to: '/settings/encounters' },
    { title: 'Encounters' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const purposesCount = computed(() => purposesStore.purposes?.length);
const procedursCount = computed(() => proceduresStore.procedures?.length);

const tabs = computed(() => [
  {
    label: 'Purposes',
    badge: purposesCount.value,
  },
  {
    label: 'Procedures',
    badge: procedursCount.value,
  },
]);

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

// Purposes //
const addPurposeModalRef = ref<InstanceType<typeof Modal>>();
const editPurposeModalRef = ref<InstanceType<typeof Modal>>();

const purposes = ref([...purposesStore.purposes]);
const activePurposes = ref<Purpose[]>(
  purposesStore.purposes.filter((purpose) => !purpose.archived)
);
const archivedPurposes = ref<Purpose[]>(
  purposesStore.purposes.filter((purpose) => purpose.archived)
);
watch(
  () => purposesStore.purposes,
  (newValue) => {
    purposes.value = [...newValue];
    activePurposes.value = newValue.filter((purpose) => !purpose.archived);
    archivedPurposes.value = newValue.filter((purpose) => purpose.archived);
    if (archivedPurposes.value.length < 1) {
      selectedPurposeGroup.value = 'Active';
    }
  },
  { deep: true }
);
const selectedPurposeGroup = ref('Active');
const purposeGroupOptions = computed(() => [
  { value: 'Active', label: 'Active', count: activePurposes.value.length },
  { value: 'Inactive', label: 'Inactive', count: archivedPurposes.value.length },
]);

const filteredPurposes = ref<Purpose[]>([]);
const updateFilteredPurposes = () => {
  filteredPurposes.value = purposes.value.filter((purpose) => {
    return (
      (!purpose.archived && selectedPurposeGroup.value === 'Active') ||
      (purpose.archived && selectedPurposeGroup.value === 'Inactive')
    );
  });
};
updateFilteredPurposes();
watch([purposes, selectedPurposeGroup], updateFilteredPurposes, { deep: true });

const sortPurpose = (e: any) => {
  const draggedItemId = e.item.dataset.id;
  purposesStore.sortPurpose({
    id: draggedItemId,
    from: e.oldIndex + 1,
    to: e.newIndex + 1,
  });
};

const schemaAddPurpose = yup.object({
  purposeName: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitAddPurpose } = useForm({
  validationSchema: schemaAddPurpose,
});
const {
  value: purposeName,
  errorMessage: purposeNameError,
  resetField: resetPurposeName,
} = useField<string>('purposeName');
const listPlacement = ref('first');

function handleAddPurpose() {
  handleSubmitAddPurpose(async () => {
    const newPurpose: CreatePurposeCmd = {
      name: purposeName.value,
      insertOnTop: listPlacement.value.includes('first'),
    };
    purposesStore.createPurpose(newPurpose);
    resetPurposeName();
    addPurposeModalRef.value?.setModalOpen(false);
  })();
}
function handleCancelAddPurpose() {
  addPurposeModalRef.value?.setModalOpen(false);
}

// delete/archive/restore purpose
const actionablePurpose = ref<Purpose>();
const deletePurposeModalRef = ref<InstanceType<typeof DeletePurposeModal>>();
const archivePurposeModalRef = ref<InstanceType<typeof ArchivePurposeModal>>();
const restorePurposeModalRef = ref<InstanceType<typeof RestorePurposeModal>>();

const handleDeletePurpose = (purpose: Purpose) => {
  if (purpose.references && purpose.references > 0) {
    actionablePurpose.value = purpose;
    nextTick(() => {
      archivePurposeModalRef?.value?.setModalOpen(true);
    });
  } else {
    actionablePurpose.value = purpose;
    nextTick(() => {
      deletePurposeModalRef?.value?.setModalOpen(true);
    });
  }
};

const handleRestorePurpose = (purpose: Purpose) => {
  if (purpose.archived) {
    actionablePurpose.value = purpose;
    nextTick(() => {
      restorePurposeModalRef?.value?.setModalOpen(true);
    });
  }
};

// edit purpose

const schemaEditPurpose = yup.object({
  purposeNameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitEditPurpose } = useForm({
  validationSchema: schemaEditPurpose,
});
const { value: purposeNameEdit, errorMessage: purposeNameEditError } =
  useField<string>('purposeNameEdit');

const activePurposeId = ref('');
const hadleClickPurpose = (id: string) => {
  const purpose = purposes.value.find((item) => item.id === id);
  if (purpose) {
    activePurposeId.value = purpose.id;
    purposeNameEdit.value = purpose.name!;
    editPurposeModalRef.value?.setModalOpen(true);
  }
};
const handleEditPurpose = () => {
  handleSubmitEditPurpose(async () => {
    const updatedPurpose = {
      id: activePurposeId.value,
      name: purposeNameEdit.value,
    };
    purposesStore.renamePurpose(updatedPurpose);
    editPurposeModalRef.value?.setModalOpen(false);
  })();
};

function handleCancelEditPurpose() {
  editPurposeModalRef.value?.setModalOpen(false);
}

// Procedures
const addProcedureModal = ref<InstanceType<typeof Modal>>();
const procedures = ref([...proceduresStore.procedures]);
const activeProcedures = ref<Procedure[]>(
  proceduresStore.procedures.filter((procedure) => !procedure.archived)
);
const archivedProcedures = ref<Procedure[]>(
  proceduresStore.procedures.filter((procedure) => procedure.archived)
);
watch(
  () => proceduresStore.procedures,
  (newValue) => {
    procedures.value = [...newValue];
    activeProcedures.value = newValue.filter((procedure) => !procedure.archived);
    archivedProcedures.value = newValue.filter((procedure) => procedure.archived);
    if (archivedProcedures.value.length < 1) {
      selectedProcedureGroup.value = 'Active';
    }
  },
  { deep: true }
);
const selectedProcedureGroup = ref('Active');
const procedureGroupOptions = computed(() => [
  { value: 'Active', label: 'Active', count: activeProcedures.value.length },
  { value: 'Inactive', label: 'Inactive', count: archivedProcedures.value.length },
]);

const filteredProcedures = ref<Procedure[]>([]);
const updateFilteredProcedures = () => {
  filteredProcedures.value = procedures.value.filter((procedure) => {
    return (
      (!procedure.archived && selectedProcedureGroup.value === 'Active') ||
      (procedure.archived && selectedProcedureGroup.value === 'Inactive')
    );
  });
};
updateFilteredProcedures();
watch([procedures, selectedProcedureGroup], updateFilteredProcedures, { deep: true });
const sortProcedure = (e: any) => {
  const draggedItemId = e.item.dataset.id;
  proceduresStore.sortProcedure({
    id: draggedItemId,
    from: e.oldIndex + 1,
    to: e.newIndex + 1,
  });
};

const schemaAddProcedure = yup.object({
  procedureName: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitAddProcedure } = useForm({
  validationSchema: schemaAddProcedure,
});
const { value: procedureName, errorMessage: procedureNameError } =
  useField<string>('procedureName');


const procedureType = ref('standard');

const procedureNameAsProp = ref('');
function handleAddProcedure() {
  handleSubmitAddProcedure(async () => {
    procedureNameAsProp.value = procedureName.value;
    addProcedureModal.value?.setModalOpen(false);
    createProcedureModalRef.value?.setModalOpen(true);

    // resetProcedureName();
  })();
}
function closeAddProceduresModal() {
  addProcedureModal.value?.setModalOpen(false);
}
function handleClickProcedure(procedure: Procedure) {
  proceduresStore.selectedProcedure = procedure;
  viewProcedureModalRef.value?.setModalOpen(true);
}

// drag & drop
const dragOptions = computed(() => {
  return {
    animation: 200,
    group: 'table-items',
    disabled: false,
    ghostClass: 'shadow',
  };
});

const createProcedureModalRef = ref<InstanceType<typeof CreateProcedureModal>>();
const viewProcedureModalRef = ref<InstanceType<typeof ViewProcedureModal>>();
</script>
<template>
  <div class="lg:py-10 py-4">
    <Tabs :tabs="tabs" :active-tab="tabStore.settingsEncountersActiveTab" margin="lg:mx-10 mx-4">
      <!-- Purpose tab -->
      <template #tab-0>
        <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
          <div class="max-w-3xl mx-auto lg:px-0 px-4">
            <div class="mt-4 lg:mt-10">
              <div class="font-semibold text-2xl leading-9">Purposes</div>
              <div class="text-sm font-medium leading-[19.6px]">
                Job Purposes efficiently categorize job requests with customizable job types that
                align with your unique procedures and clinical workflows, ensuring streamlined
                operations and clear identification of tasks.
              </div>
              <div class="flex items-end justify-between">
                <ButtonGroupRadio
                  v-if="archivedPurposes.length > 0"
                  v-model="selectedPurposeGroup"
                  :options="purposeGroupOptions"
                  class="mb-3"
                />

                <div class="flex justify-end items-center flex-1">
                  <button
                    @click="addPurposeModalRef?.setModalOpen(true)"
                    class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold mt-[38px] mb-3.5"
                  >
                    Add Purpose
                  </button>
                </div>
              </div>
            </div>

            <!-- table start -->
            <div class="mb-4 lg:mb-10">
              <base-table>
                <t-head class="uppercase">
                  <t-r>
                    <t-h class="w-6"></t-h>
                    <t-h class="w-16">
                      <div class="text-slate-800 flex items-center gap-2">
                        <span>Order</span>
                        <IconArrowNarrowDown />
                      </div>
                    </t-h>
                    <t-h>Name</t-h>
                    <t-h></t-h>
                  </t-r>
                </t-head>
                <draggable
                  v-if="purposes.length > 0"
                  :list="filteredPurposes"
                  item-key="id"
                  tag="tbody"
                  v-bind="dragOptions"
                  handle=".draggable-handle-icon"
                  @end="sortPurpose"
                >
                  <template #item="{ element, index }">
                    <t-r :data-id="element.id" class="group hover:bg-slate-50">
                      <t-d class="w-6">
                        <IconDotsReorder
                          class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                        />
                      </t-d>
                      <t-d>{{ index + 1 }}</t-d>
                      <t-d class="text-brand-600 font-semibold">
                        <a @click="hadleClickPurpose(element.id)" href="#">
                          {{ element.name }}
                        </a>
                      </t-d>
                      <t-d>
                        <svg
                          v-if="element.archived"
                          class="cursor-pointer block ms-auto opacity-0 group-hover:opacity-100"
                          @click="handleRestorePurpose(element)"
                          xmlns="http://www.w3.org/2000/svg"
                          width="24"
                          height="24"
                          viewBox="0 0 24 24"
                          fill="none"
                        >
                          <path
                            d="M2 10C2 10 4.00498 7.26822 5.63384 5.63824C7.26269 4.00827 9.5136 3 12 3C16.9706 3 21 7.02944 21 12C21 16.9706 16.9706 21 12 21C7.89691 21 4.43511 18.2543 3.35177 14.5M2 10V4M2 10H8"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                          />
                        </svg>
                        <IconTrash01
                          v-else
                          @click="handleDeletePurpose(element)"
                          class="cursor-pointer block ms-auto opacity-0 group-hover:opacity-100"
                        />
                      </t-d>
                    </t-r>
                  </template>
                </draggable>
                <t-body v-else>
                  <t-r>
                    <t-d colspan="5" class="text-center">No Purposes to Display</t-d>
                  </t-r>
                </t-body>
              </base-table>
            </div>
            <!-- table end -->
          </div>
        </div>
      </template>

      <!-- procedures tab -->
      <template #tab-1>
        <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
          <div class="max-w-3xl mx-auto lg:px-0 px-4">
            <div class="mt-4 lg:mt-10">
              <div class="font-semibold text-2xl leading-9">Procedures</div>
              <div class="text-sm font-medium leading-[19.6px]">
                Procedures enable the definition and management of clinical procedures with
                customizable details and settings, ensuring alignment with your standards and
                consistent, accurate documentation across all patient encounters.
              </div>
              <div class="flex items-end justify-between">
                <ButtonGroupRadio
                  v-if="archivedProcedures.length > 0"
                  v-model="selectedProcedureGroup"
                  :options="procedureGroupOptions"
                  class="mb-3"
                />
                <div class="flex justify-end items-center flex-1">
                  <button
                    @click="addProcedureModal?.setModalOpen(true)"
                    class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold mt-[38px] mb-3.5"
                  >
                    Add Procedure
                  </button>
                </div>
              </div>
            </div>

            <!-- table start -->
            <div class="mb-4 lg:mb-10">
              <base-table>
                <t-head class="uppercase">
                  <t-r>
                    <t-h class="w-6"></t-h>
                    <t-h class="w-16">
                      <div class="text-slate-800 flex items-center gap-2">
                        <span>Order </span>
                        <IconArrowNarrowDown />
                      </div>
                    </t-h>
                    <t-h>Name</t-h>
                    <t-h>Type</t-h>
                    <t-h>No OF PROPERTIES</t-h>
                  </t-r>
                </t-head>

                <draggable
                  v-if="filteredProcedures.length > 0"
                  v-model="filteredProcedures"
                  item-key="id"
                  tag="tbody"
                  v-bind="dragOptions"
                  handle=".draggable-handle-icon"
                  @end="sortProcedure"
                >
                  <template #item="{ element: item, index }">
                    <t-r :data-id="item.id" class="group hover:bg-slate-50">
                      <t-d class="w-6">
                        <IconDotsReorder
                          class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                      /></t-d>
                      <t-d>{{ index + 1 }}</t-d>
                      <t-d class="text-brand-600 font-semibold">
                        <a @click="handleClickProcedure(item)" href="#">
                          {{ item.name }}
                        </a>
                      </t-d>
                      <t-d class="capitalize">
                        <span
                          v-if="item.type === ProcedureType.Insertion"
                          class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 bg-green-100 text-green-700"
                          >Insertion</span
                        >
                        <span
                          v-if="item.type == ProcedureType.Removal"
                          class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 bg-red-100 text-red-700"
                          >Removal</span
                        >
                      </t-d>
                      <t-d>{{ item.fields.length }}</t-d>
                    </t-r>
                  </template>
                </draggable>
                <t-body v-else>
                  <t-r>
                    <t-d colspan="5" class="text-center">No Procedures to Display</t-d>
                  </t-r>
                </t-body>
              </base-table>
            </div>
            <!-- table end -->
          </div>
        </div>
      </template>
    </Tabs>
    <!-- add Purpose modal -->
    <teleport to="body">
      <Modal ref="addPurposeModalRef" title="Add Purpose" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Name</label
              >
              <fwb-input
                v-model.trim="purposeName"
                placeholder="enter purpose name ..."
                :class="purposeNameError ? inputErrorClasses : ''"
              />
              <span v-if="purposeNameError" class="text-sm text-radical-red-600">{{
                purposeNameError
              }}</span>
            </div>
            <div class="text-sm font-medium text-[#111928]">List Placement</div>
            <div class="flex items-center">
              <fwb-radio v-model="listPlacement" label="Top (First)" value="first" />
              <fwb-radio v-model="listPlacement" label="Bottom (Last)" value="last" />
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="handleCancelAddPurpose" color="light" pill> Cancel</fwb-button>
            <fwb-button
              @click.prevent="handleAddPurpose"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              Save & Close
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
    <!-- edit Purpose modal -->
    <teleport to="body">
      <Modal ref="editPurposeModalRef" title="Edit Purpose" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Name</label
              >
              <fwb-input
                v-model.trim="purposeNameEdit"
                placeholder="enter Purpose name ..."
                :class="purposeNameEditError ? inputErrorClasses : ''"
              />
              <span v-if="purposeNameEditError" class="text-sm text-radical-red-600">{{
                purposeNameEditError
              }}</span>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="handleCancelEditPurpose" color="light" pill> Cancel</fwb-button>
            <fwb-button @click="handleEditPurpose" class="bg-primary-600 hover:bg-brand-600" pill>
              Save & close
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
    <!-- add procedures modal -->
    <teleport to="body">
      <Modal ref="addProcedureModal" title="Add Procedure" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-[#0F172A] dark:text-white"
                >Name</label
              >
              <fwb-input
                v-model.trim="procedureName"
                placeholder="enter procedure name ..."
                :class="procedureNameError ? inputErrorClasses : ''"
              />
              <span v-if="procedureNameError" class="text-sm text-radical-red-600">{{
                procedureNameError
              }}</span>
            </div>
            <div>
              <label class="mb-2 block text-sm font-medium text-[#0F172A] dark:text-white"
                >Procedure Type</label
              >
              <div class="flex flex-col gap-2">
                <ButtonRadio v-model="procedureType" :value="ProcedureType.Standard" label="Standard"/>
                <ButtonRadio v-model="procedureType" :value="ProcedureType.Insertion" label="Insertion"/>
                <ButtonRadio v-model="procedureType" :value="ProcedureType.Removal" label="Removal"/>
              </div>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="closeAddProceduresModal" color="light" pill> Cancel</fwb-button>
            <fwb-button @click="handleAddProcedure" class="bg-primary-600 hover:bg-brand-600" pill>
              Continue
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
    <CreateProcedureModal ref="createProcedureModalRef" :name="procedureNameAsProp" :type="procedureType"/>
    <ViewProcedureModal
      ref="viewProcedureModalRef"
      :procedure="proceduresStore.selectedProcedure"
    />
    <ArchivePurposeModal
      v-if="actionablePurpose"
      ref="archivePurposeModalRef"
      :purpose="actionablePurpose"
    />
    <DeletePurposeModal
      v-if="actionablePurpose"
      ref="deletePurposeModalRef"
      :purpose="actionablePurpose"
    />
    <RestorePurposeModal
      v-if="actionablePurpose"
      ref="restorePurposeModalRef"
      :purpose="actionablePurpose"
    />
  </div>
</template>
