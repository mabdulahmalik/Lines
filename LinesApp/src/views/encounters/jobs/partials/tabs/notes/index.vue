<script setup lang="ts">
import { FwbButton, FwbInput, FwbCheckbox } from 'flowbite-vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import { ref, watch, computed } from 'vue';
import {
  IconDotHorizontal,
  IconEdit05,
  IconTrash01,
  IconPapperPlane,
  IconFileAttachment03,
  IconFileUnattachment03,
  IconPin02,
  IconUnpin,
} from '@/components/icons/index';
import { Encounter, Job, JobStatus, User } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { useUsersStore } from '@/stores/data/settings/users';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import UserAvatar from '@/components/avatar/UserAvatar.vue';
import DateTimeFormatter from '@/utils/dateTimeFormatter';

const props = defineProps<{
  job: Job;
  encounter?: Encounter;
}>();

const jobsStore = useJobsStore();
const usersStore = useUsersStore();
const medicalRecordsStore = useMedicalRecordsStore();

const selectedMedicalRecord = computed(() => {
  return medicalRecordsStore.medicalRecords.find(
    (medicalRecord) => medicalRecord.id === props.job.medicalRecordId
  );
});

const allowEdit = computed(() => props.job.status !== JobStatus.Canceled);

const pinNote = ref(false);
const markAsObservation = ref(false);

const notes = ref(
  (props.job.notes?.filter((note) => note !== null) || [])
    .map((note) => ({
      ...note,
      observation:
        selectedMedicalRecord.value?.observations?.some(
          (observation) => observation?.objectId === note.id
        ) ?? false,
    }))
    .sort((a, b) => {
      if (a.pinned && !b.pinned) return -1;
      if (!a.pinned && b.pinned) return 1;
      return new Date(b.createdAt as string).getTime() - new Date(a.createdAt as string).getTime();
    })
);

watch(
  () => props.job.notes,
  (newNotes) => {
    notes.value = (newNotes?.filter((note) => note !== null) || [])
      .map((note) => ({
        ...note,
        observation:
          selectedMedicalRecord.value?.observations?.some(
            (observation) => observation?.objectId === note.id
          ) ?? false,
      }))
      .sort((a, b) => {
        if (a.pinned && !b.pinned) return -1;
        if (!a.pinned && b.pinned) return 1;
        return (
          new Date(b.createdAt as string).getTime() - new Date(a.createdAt as string).getTime()
        );
      });
  },
  { immediate: true, deep: true }
);

const note = ref('');
const isEditing = ref(false);
const editingNoteId = ref<string | null>(null);

function addOrEditNote() {
  if (isEditing.value && editingNoteId.value) {
    if (note.value && props.job.id) {
      jobsStore.modifyNoteForJob({
        id: editingNoteId.value,
        jobId: props.job.id,
        text: note.value,
      });
    }
    isEditing.value = false;
    editingNoteId.value = null;
  } else {
    if (note.value && props.job.id) {
      jobsStore.attachNoteToJob({
        jobId: props.job.id,
        medicalRecordId: markAsObservation.value ? props.job.medicalRecordId : null,
        pinned: pinNote.value,
        text: note.value,
      });
    }
  }
  note.value = '';
  pinNote.value = false;
  markAsObservation.value = false;
}

const pinnedUnpinNotes = (id: string) => {
  const note = notes.value.find((n) => n.id === id);
  if (note && props.job.id) {
    if (note.pinned) {
      jobsStore.unpinNoteFromJob({ id, jobId: props.job.id });
    } else {
      jobsStore.pinNoteToJob({ id, jobId: props.job.id });
    }
  }
};

// observation

const makeAndDiscardObservationNotes = (id: string) => {
  const noteIndex = notes.value.findIndex((n) => n.id === id);
  if (noteIndex !== -1 && props.job.id) {
    const note = notes.value[noteIndex];
    if (note.observation) {
      jobsStore.discardNoteAsObservation({
        id: id,
        jobId: props.job.id,
        medicalRecordId: props.job.medicalRecordId,
      });
    } else {
      jobsStore.makeNoteAnObservation({
        id: id,
        jobId: props.job.id,
        medicalRecordId: props.job.medicalRecordId,
      });
    }
    notes.value.splice(noteIndex, 1, {
      ...note,
      observation: !note.observation,
    });
  }
};

function deleteNote(id: string) {
  if (props.job.id && id) {
    jobsStore.removeNoteFromJob({ id: id, jobId: props.job.id });
  }
}

function editNote(id: string) {
  const noteIndex = notes.value.findIndex((note) => note.id === id);
  if (noteIndex !== -1) {
    editingNoteId.value = id;
    note.value = notes.value[noteIndex].text || '';
    isEditing.value = true;
  }
}

function getUserName(userId: string): string {
  const user = usersStore.users.find((user) => user.id === userId);
  return user ? user.firstName + ' '+ user.lastName : '';
}
function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}
</script>

<template>
  <div class="max-w-3xl mx-auto">
    <!-- Add/Edit Note -->
    <div v-if="allowEdit" class="border-b border-slate-300 pb-6 mb-4">
      <div class="flex flex-col p-3 bg-slate-50 rounded-lg">
        <div class="flex items-center">
          <fwb-input
            v-model="note"
            @enter=""
            class="flex-1"
            block-classes="flex-1 bg-white"
            placeholder="Write text here ..."
          />
          <fwb-button
            @click="addOrEditNote"
            type="submit"
            square
            color="light"
            class="bg-transparent border-transparent hover:bg-transparent focus:ring-0"
          >
            <IconPapperPlane class="text-brand-700" />
          </fwb-button>
        </div>
        <div class="flex gap-2 gap-x-4 pt-3">
          <fwb-checkbox v-model="pinNote" label="Pin to notes" />
          <fwb-checkbox
            v-if="props.encounter?.medicalRecordId"
            v-model="markAsObservation"
            label="Mark as ‘Observation’"
          />
        </div>
      </div>
    </div>
    <!-- Notes -->
    <template v-if="notes">
      <div
        v-for="(note, index) in notes"
        :key="index"
        class="flex border-b border-slate-300 pb-4 mb-4"
      >
        <div
          class="flex w-full px-2"
          :class="{ 'bg-[#FDFDEA] py-2 rounded-xl': note.pinned }"
        >
          <user-avatar :user="getUserForAvatar(note.createdBy)" rounded size="sm" class="mr-3" />
          <div class="flex flex-col flex-1">
            <div class="flex items-start justify-between">
              <div>
                <div class="text-slate-500 font-medium text-xs">
                  {{ DateTimeFormatter.formatDatetime(note.createdAt) }}
                </div>
                <div class="text-slate-900 font-semibold text-sm">
                  {{ getUserName(note.createdBy) }}
                </div>
              </div>
              <div class="flex items-center gap-1">
                <!-- observation -->
                <fwb-button
                  v-if="allowEdit && props.encounter?.medicalRecordId"
                  @click="makeAndDiscardObservationNotes(note.id)"
                  color="light"
                  class="border-transparent bg-transparent focus:ring-0"
                  pill
                  square
                  size="sm"
                >
                  <template v-if="note.observation">
                    <IconFileAttachment03 />
                  </template>
                  <template v-else>
                    <IconFileUnattachment03 />
                  </template>
                </fwb-button>
                <!-- Pinned Unpinned -->
                <fwb-button
                  v-if="allowEdit"
                  @click="pinnedUnpinNotes(note.id)"
                  color="light"
                  class="border-transparent bg-transparent focus:ring-0"
                  pill
                  square
                  size="sm"
                >
                  <template v-if="note.pinned">
                    <IconUnpin />
                  </template>
                  <template v-else>
                    <IconPin02 />
                  </template>
                </fwb-button>
                <Dropdown v-if="allowEdit" align-to-end class="rounded-lg" close-inside>
                  <template #trigger>
                    <fwb-button
                      color="light"
                      class="border-transparent bg-transparent focus:ring-0"
                      pill
                      square
                      size="sm"
                    >
                      <IconDotHorizontal />
                    </fwb-button>
                  </template>
                  <DropdownMenu class="min-w-36 max-w-36">
                    <DropdownItem @click="editNote(note.id)">
                      <IconEdit05 class="mr-2" />
                      Edit
                    </DropdownItem>
                    <DropdownItem @click="deleteNote(note.id)">
                      <IconTrash01 class="mr-2" />
                      Delete
                    </DropdownItem>
                  </DropdownMenu>
                </Dropdown>
              </div>
            </div>
            <div class="font-medium text-sm pt-2 text-gray-900">
              {{ note.text }}
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<style scoped>
:deep(div.flex.relative.items-center.bg-gray-50.border.border-gray-300:has(input)) {
  background-color: white;
}
</style>
