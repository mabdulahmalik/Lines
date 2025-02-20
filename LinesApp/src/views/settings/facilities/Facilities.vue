<script setup lang="ts">
import Tabs from '@/components/tabs/Tabs.vue';
import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
import Modal from '@/components/modal/Modal.vue';
import { FwbInput, FwbButton, FwbSelect, FwbToggle, FwbTooltip } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { IconPlusCircle, IconArrowNarrowDown, IconDotsReorder } from '@/components/icons/index';
import { BaseTable, THead, TR, TD, TH, TBody } from '@/components/table/index';
import draggable from 'vuedraggable';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useForm, useField, useFieldArray } from 'vee-validate';
import * as yup from 'yup';
import { FacilityRoomProperty } from '@/api/__generated__/graphql';
import ViewRoomPropertyModal from './modal/ViewRoomPropertyModal.vue';
import Routines from './routines/Routines.vue';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { useTabStore } from '@/stores/tab';
interface RoomInputField {
  value: {
    name: string;
  };
}
const breadcrumbStore = useBreadcrumbStore();
const facilityTypesStore = useFacilityTypesStore();
const routinesStore = useRoutinesStore();
const tabStore = useTabStore();
onMounted(() => {
  tabStore.settingsFacilitiesActiveTab = 0;
});

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'Settings', to: '/settings/encounters' },
    { title: 'Facilities' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const fTCount = computed(() => facilityTypesStore.facilityTypes?.length);
const rPCount = computed(() => facilityTypesStore.roomProperties?.length);
const routinesCount = computed(() => routinesStore.routines?.length);
const tabs = computed(() => [
  {
    label: 'Facility Types',
    badge: fTCount.value,
  },
  {
    label: 'Room Properties',
    badge: rPCount.value,
  },
  {
    label: 'Routines',
    badge: routinesCount.value,
  },
]);

// Facility Types

const modalFacilityRef = ref<InstanceType<typeof Modal>>();

// We are using v-model on facilityTypes
const facilityTypes = ref([...facilityTypesStore.facilityTypes]);

watch(
  () => facilityTypesStore.facilityTypes,
  (newValue) => {
    facilityTypes.value = [...newValue];
  },
  { deep: true }
);

const onDragEndFacilityTypes = (evt: any) => {
  const { oldIndex, newIndex } = evt;
  const draggedItemId = evt.item.dataset.id;
  const payload = {
    from: oldIndex + 1,
    id: draggedItemId,
    to: newIndex + 1,
  };
  facilityTypesStore.sortFacilityType(payload);
  // console.log('Item position changed:', payload);
};

const schemaFacilityType = yup.object({
  facilityTypeName: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSaveFacility } = useForm({
  validationSchema: schemaFacilityType,
});
const {
  value: facilityTypeName,
  errorMessage: facilityTypeNameError,
  resetField: resetFacilityTypeName,
} = useField<string>('facilityTypeName');

function handleCancelFacility() {
  modalFacilityRef.value?.setModalOpen(false);
}
function handleAddFacility() {
  handleSaveFacility(async () => {
    const newFacility = {
      name: facilityTypeName.value,
    };
    facilityTypesStore.createFacilityType(newFacility);
    resetFacilityTypeName();
    modalFacilityRef.value?.setModalOpen(false);
  })();
}

// Edit facility type
const editFacilityTypeModalRef = ref<InstanceType<typeof Modal>>();
const schemaEditFacilityType = yup.object({
  facilityTypeNameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitEditFacilityType } = useForm({
  validationSchema: schemaEditFacilityType,
});
const { value: facilityTypeNameEdit, errorMessage: facilityTypeNameEditError } =
  useField<string>('facilityTypeNameEdit');

const activeFacilityTypeId = ref('');
const hadleClickFacilityType = (id: string) => {
  const facilityType = facilityTypes.value.find((item) => item.id === id);
  if (facilityType) {
    activeFacilityTypeId.value = facilityType.id;
    facilityTypeNameEdit.value = facilityType.name!;
    editFacilityTypeModalRef.value?.setModalOpen(true);
  }
};
const handleEditFacilityType = () => {
  handleSubmitEditFacilityType(async () => {
    const updatedFacilityType = {
      id: activeFacilityTypeId.value,
      name: facilityTypeNameEdit.value,
    };
    facilityTypesStore.renameFacilityType(updatedFacilityType);
    editFacilityTypeModalRef.value?.setModalOpen(false);
  })();
};

function handleCancelEditFacilityType() {
  editFacilityTypeModalRef.value?.setModalOpen(false);
}

// Room Properties / Facility Type Properties
const modalAddRoomRef = ref<InstanceType<typeof ModalDrawer>>();
// const facilityTypeProperties = computed(() => facilityTypesStore.roomProperties);
// We are using v-model on facilityTypeProperties
const facilityTypeProperties = ref([...facilityTypesStore.roomProperties]);
watch(
  () => facilityTypesStore.roomProperties,
  (newValue) => {
    facilityTypeProperties.value = [...newValue];
  },
  { deep: true }
);

const schemaFacilityTypeProperty = yup.object({
  facilityTypePropertyName: yup.string().required('This is a required field').trim(),
  associatedFacilityTypeId: yup.string().required('This is a required field'),
  roomInputFields: yup.array().of(
    yup.object().shape({
      name: yup.string().required('This field is required').trim(),
    })
  ),
});
const initialRoomInputFields = [{ name: '' }];

const { handleSubmit: handleSaveFTProperty, errors: createRoomErrors } = useForm({
  initialValues: {
    roomInputFields: initialRoomInputFields,
    facilityTypePropertyName: '',
  },
  validationSchema: schemaFacilityTypeProperty,
});
const {
  value: facilityTypePropertyName,
  errorMessage: facilityTypePropertyNameError,
  resetField: resetFacilityTypePropertyName,
} = useField<string>('facilityTypePropertyName');
const { value: associatedFacilityTypeId, errorMessage: associatedFacilityTypeIdError } =
  useField<string>('associatedFacilityTypeId');

const {
  fields: roomInputFields,
  push: pushToRoomInputFields,
  replace: replaceRoomInputFields,
} = useFieldArray('roomInputFields');

const addInputField = () => {
  // isAddFTPropertySubmit.value = false;
  pushToRoomInputFields({ name: '' });
};

const associateFacilityOptions = computed(() =>
  facilityTypes.value.map((r) => ({
    value: r.id,
    name: r.name!,
  }))
);
const isAddFTPropertySubmit = ref(false);
function handleAddFTProperty() {
  isAddFTPropertySubmit.value = true;
  handleSaveFTProperty(async () => {
    const newFTProperty = {
      facilityTypeId: associatedFacilityTypeId.value,
      name: facilityTypePropertyName.value,
      options: roomInputFields.value.map((item) => (item.value as RoomInputField['value']).name),
    };
    facilityTypesStore.addRoomProperty(newFTProperty);
    isAddFTPropertySubmit.value = false;
    resetFacilityTypePropertyName();
    replaceRoomInputFields([{ name: '' }]);
    modalAddRoomRef.value?.setModalOpen(false);
  })();
}
function handleCancelAddRoom() {
  modalAddRoomRef.value?.setModalOpen(false);
}

// Edit/View room properties
const viewRoomPropertyModalComRef = ref<InstanceType<typeof ViewRoomPropertyModal>>();
function openViewRoomModal(item: FacilityRoomProperty) {
  facilityTypesStore.selectedRoomProperty = item;
  viewRoomPropertyModalComRef.value?.setModalOpen(true);
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

function onToggleChange(facilityType: any) {
  if (!facilityType.isActive) {
    facilityTypesStore.activateFacilityType({ id: facilityType.id });
    return;
  }
  facilityTypesStore.deactivateFacilityType({ id: facilityType.id });
}

const sortRoomProperty = (e: any) => {
  const roomProperty = facilityTypeProperties.value[e.newIndex] as any;
  
  facilityTypesStore.sortRoomProperty({
    id: roomProperty.id,
    facilityTypeId: roomProperty.facilityTypeId,
    from: e.oldIndex + 1,
    to: e.newIndex + 1,
  });
};

</script>
<template>
  <div class="lg:py-10 py-4">
    <Tabs :tabs="tabs" :active-tab="tabStore.settingsFacilitiesActiveTab" margin="lg:mx-10 mx-4">
      <!-- Facility Types tab -->
      <template #tab-0>
        <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
          <div class="max-w-3xl mx-auto lg:0 px-4">
            <div class="mt-4 lg:mt-10">
              <div class="font-semibold text-2xl leading-9">Facility Types</div>
              <div class="text-sm font-medium leading-[19.6px]">
                Facility Types help to classify the different facilities being serviced by your
                team. They contain Room Properties that can be used to describe and define the
                organizational structure of your facilities.
              </div>
              <div class="flex justify-end">
                <button
                  @click="modalFacilityRef?.setModalOpen(true)"
                  class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold mt-[38px] mb-3.5"
                >
                  Add Facility Type
                </button>
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
                    <t-h></t-h>
                  </t-r>
                </t-head>
                <draggable
                  v-if="facilityTypes.length > 0"
                  v-model="facilityTypes"
                  item-key="id"
                  tag="tbody"
                  v-bind="dragOptions"
                  handle=".draggable-handle-icon"
                  @end="onDragEndFacilityTypes"
                >
                  <template #item="{ element: item, index }">
                    <t-r :data-id="item.id" class="hover:bg-slate-50">
                      <t-d class="w-6">
                        <IconDotsReorder
                          class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                      /></t-d>
                      <t-d> {{ index + 1 }}</t-d>
                      <t-d class="text-brand-600 font-semibold">
                        <a @click="hadleClickFacilityType(item.id)" href="#">
                          {{ item.name }}
                        </a>
                      </t-d>
                      <t-d>
                        <div class="flex justify-end items-center">
                          <fwb-tooltip>
                            <template #trigger>
                              <FwbToggle
                                :model-value="item.isActive"
                                @change="onToggleChange(item)"
                                color="purple"
                              />
                            </template>
                            <template #content>
                              <span class="text-sm font-medium">
                                <template v-if="item.isActive"> De-activate</template>
                                <template v-else> Activate</template>
                              </span>
                            </template>
                          </fwb-tooltip>
                        </div>
                      </t-d>
                    </t-r>
                  </template>
                </draggable>
                <t-body v-else>
                  <t-r>
                    <t-d colspan="4" class="text-center">No Facility Types to Display</t-d>
                  </t-r>
                </t-body>
              </base-table>
            </div>
            <!-- table end -->
          </div>
        </div>
      </template>
      <!-- Room properties tab -->
      <template #tab-1>
        <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
          <div class="max-w-3xl mx-auto lg:0 px-4">
            <div class="mt-4 lg:mt-10">
              <div class="font-semibold text-2xl leading-9">Room Properties</div>
              <div class="text-sm font-medium leading-[19.6px]">
                Room Properties are leveraged by Facility Types to describe and define the
                organizational structure of your facilities. A Room Property can help to describe
                different aspects of a room within a facility, such as where it's located, what it's
                used for, and more.
              </div>
              <div class="flex justify-end">
                <button
                  @click="modalAddRoomRef?.setModalOpen(true)"
                  class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold mt-[38px] mb-3.5"
                >
                  Add Room Property
                </button>
              </div>
            </div>

            <!-- table start -->
            <div class="mb-4 lg:mb-10">
              <base-table>
                <t-head class="uppercase">
                  <t-r class="hover:bg-slate-50">
                    <t-h class="w-6"></t-h>
                    <t-h class="w-16">
                      <div class="text-slate-800 flex items-center gap-2">
                        <span>Order </span>
                        <IconArrowNarrowDown />
                      </div>
                    </t-h>
                    <t-h>Name</t-h>
                    <t-h>No OF Values</t-h>
                    <t-h>Facility type</t-h>
                  </t-r>
                </t-head>

                <draggable
                  v-if="facilityTypeProperties.length > 0"
                  v-model="facilityTypeProperties"
                  item-key="id"
                  tag="tbody"
                  v-bind="dragOptions"
                  handle=".draggable-handle-icon"
                  @end="sortRoomProperty"
                >
                  <template #item="{ element: item, index }">
                    <t-r class="hover:bg-slate-50">
                      <t-d class="w-6">
                        <IconDotsReorder
                          class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                      /></t-d>
                      <t-d>{{ index + 1 }}</t-d>
                      <t-d class="text-brand-600 font-semibold">
                        <a href="#" @click="openViewRoomModal(item)">
                          {{ item.name }}
                        </a>
                      </t-d>
                      <t-d>{{ item.options?.length }}</t-d>
                      <t-d>
                        <span
                          class="text-xs bg-[#DEF7EC] text-[#057A55] px-[10px] py-[2px] leading-[18px] font-medium rounded-full"
                        >
                          {{ item.facilityType }}
                        </span>
                      </t-d>
                    </t-r>
                  </template>
                </draggable>
                <t-body v-else>
                  <t-r>
                    <t-d colspan="5" class="text-center">No Room Properties to Display</t-d>
                  </t-r>
                </t-body>
              </base-table>
            </div>
            <!-- table end -->
          </div>
        </div>
      </template>
      <!-- Routines tab -->
      <template #tab-2>
        <Routines />
      </template>
    </Tabs>

    <!-- Add facility type modal -->
    <teleport to="body">
      <Modal ref="modalFacilityRef" title="Add Facility Type" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Name</label
              >
              <fwb-input
                v-model.trim="facilityTypeName"
                placeholder="input name here ..."
                :class="facilityTypeNameError ? inputErrorClasses : ''"
              />
              <span v-if="facilityTypeNameError" class="text-sm text-radical-red-600">{{
                facilityTypeNameError
              }}</span>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="handleCancelFacility" color="light" pill> Cancel</fwb-button>
            <fwb-button
              @click.prevent="handleAddFacility"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              Save & Close
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
    <!-- Edit/Rename facility Type modal -->
    <teleport to="body">
      <Modal ref="editFacilityTypeModalRef" title="Edit Facility Type" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Name</label
              >
              <fwb-input
                v-model.trim="facilityTypeNameEdit"
                placeholder="input name here ..."
                :class="facilityTypeNameEditError ? inputErrorClasses : ''"
              />
              <span v-if="facilityTypeNameEditError" class="text-sm text-radical-red-600">{{
                facilityTypeNameEditError
              }}</span>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="handleCancelEditFacilityType" color="light" pill>
              Cancel</fwb-button
            >
            <fwb-button
              @click="handleEditFacilityType"
              class="bg-primary-600 hover:bg-brand-600"
              pill
            >
              Save & Close
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
    <!-- Add room property drawer -->
    <ModalDrawer ref="modalAddRoomRef" close_outside max_width="xl" title="Add Room Property">
      <template #body>
        <!-- body -->
        <div class="flex flex-col px-4 py-2 lg:px-8 lg:py-8">
          <div>
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Property Name</label
            >
            <fwb-input
              v-model="facilityTypePropertyName"
              placeholder="Write text here"
              :class="facilityTypePropertyNameError ? inputErrorClasses : ''"
            />
            <span v-if="facilityTypePropertyNameError" class="text-sm text-radical-red-600">{{
              facilityTypePropertyNameError
            }}</span>
          </div>

          <div class="mt-4">
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Associate with this Facility Type</label
            >
            <fwb-select
              v-model="associatedFacilityTypeId"
              :options="associateFacilityOptions"
              :class="associatedFacilityTypeIdError ? inputErrorClasses : ''"
            />
            <span v-if="associatedFacilityTypeIdError" class="text-sm text-radical-red-600">{{
              associatedFacilityTypeIdError
            }}</span>
          </div>
          <hr class="mt-6 text-slate-300" />
          <div class="my-6">
            <div class="text-xl font-semibold">Add Values</div>
            <!-- input box -->
            <div v-for="(field, index) in roomInputFields" :key="field.key" class="my-4">
              <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                >Value {{ index + 1 }}</label
              >
              <fwb-input
                v-model="(field.value as RoomInputField['value']).name"
                placeholder="Write text here"
                :class="createRoomErrors[`roomInputFields[${index}].name` as keyof typeof createRoomErrors] && isAddFTPropertySubmit ? inputErrorClasses : ''"
              />
              <span
                v-if="createRoomErrors[`roomInputFields[${index}].name` as keyof typeof createRoomErrors] && isAddFTPropertySubmit"
                class="text-sm text-radical-red-600"
                >{{
                  createRoomErrors[
                    `roomInputFields[${index}].name` as keyof typeof createRoomErrors
                  ]
                }}</span
              >
            </div>
          </div>
          <hr class="mb-4 -mt-4 text-slate-300" />
          <button
            @click="addInputField"
            class="flex items-center text-brand-600 text-sm font-medium"
          >
            <IconPlusCircle class="mr-0.5" /> Add Value
          </button>
        </div>
      </template>
      <template #footer>
        <div class="p-4 lg:px-6 flex items-center justify-end w-full">
          <div class="flex items-center gap-4 lg:gap-6">
            <span
              v-if="Object.keys(createRoomErrors).length && isAddFTPropertySubmit"
              class="text-sm font-normal text-radical-red-600"
              >{{ Object.keys(createRoomErrors).length }} errors</span
            >
            <fwb-button @click="handleCancelAddRoom" color="light" pill> Cancel</fwb-button>
            <fwb-button
              @click.prevent="handleAddFTProperty"
              class="bg-primary-600 hover:bg-brand-600 w-full lg:w-auto"
              pill
            >
              Save
            </fwb-button>
          </div>
        </div>
      </template>
    </ModalDrawer>

    <!-- View room property drawer -->
    <ViewRoomPropertyModal
      ref="viewRoomPropertyModalComRef"
      :roomProperty="facilityTypesStore.selectedRoomProperty"
    />
  </div>
</template>

<style scoped>
/* Select flowbite validition classes */
:deep(.bg-radical-red-50 select) {
  border-color: #e11d47;
  background-color: inherit;
}
</style>
