<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import { ref, watch, computed } from 'vue';
import { IconDotHorizontal, IconTrash01 } from '@/components/icons/index';
import { Encounter, Job, User } from '@/api/__generated__/graphql';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { useUsersStore } from '@/stores/data/settings/users';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import UserAvatar from '@/components/avatar/UserAvatar.vue';
import DateTimeFormatter from '@/utils/dateTimeFormatter';

const props = defineProps<{
  job: Job;
  encounter: Encounter;
}>();

const jobsStore = useJobsStore();
const usersStore = useUsersStore();
const medicalRecordsStore = useMedicalRecordsStore();

const selectedMedicalRecord = computed(() => {
  return medicalRecordsStore.medicalRecords.find(
    (medicalRecord) => medicalRecord.id === props.job.medicalRecordId
  );
});

const observations = ref(
  (
    props.job.notes?.filter(
      (note) =>
        note !== null &&
        selectedMedicalRecord.value?.observations?.some(
          (observation) => observation?.objectId === note.id
        )
    ) || []
  ).sort((a: any, b: any) => {
    return new Date(b.createdAt as string).getTime() - new Date(a.createdAt as string).getTime();
  })
);

watch(
  () => props.job.notes,
  (newNotes) => {
    observations.value = (
      newNotes?.filter(
        (note) =>
          note !== null &&
          selectedMedicalRecord.value?.observations?.some(
            (observation) => observation?.objectId === note.id
          )
      ) || []
    ).sort((a: any, b: any) => {
      return new Date(b.createdAt as string).getTime() - new Date(a.createdAt as string).getTime();
    });
  }
);

function deleteNote(id: string) {
  if (props.job.id && id) {
    jobsStore.removeNoteFromJob({ id: id, jobId: props.job.id });
  }
}

function getUserName(userId: string): string {
  const user = usersStore.users.find((user) => user.id === userId);
  if (user) {
    return user.firstName + ' ' + user.lastName;
  }
  return '';
}
function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}
</script>

<template>
  <div class="max-w-3xl mx-auto">
    <!-- observations -->
    <template v-if="observations">
      <div
        v-for="(observation, index) in observations"
        :key="index"
        class="flex border-b border-slate-300 pb-6 mb-6"
      >
        <user-avatar
          :user="getUserForAvatar(observation?.createdBy)"
          rounded
          size="sm"
          class="mr-3"
        />
        <div class="flex flex-col flex-1">
          <div class="flex items-start justify-between">
            <div>
              <div class="text-slate-500 font-medium text-xs">
                {{ DateTimeFormatter.formatDatetime(observation?.createdAt) }}
              </div>
              <div class="text-slate-900 font-semibold text-sm">
                {{ getUserName(observation?.createdBy) }}
              </div>
            </div>
            <div class="flex items-center gap-1">
              <Dropdown align-to-end class="rounded-lg" close-inside>
                <template #trigger>
                  <fwb-button color="light" class="border-white focus:ring-0" pill square size="sm">
                    <IconDotHorizontal />
                  </fwb-button>
                </template>
                <DropdownMenu class="min-w-36 max-w-36">
                  <DropdownItem @click="deleteNote(observation?.id)">
                    <IconTrash01 class="mr-2" />
                    Delete
                  </DropdownItem>
                </DropdownMenu>
              </Dropdown>
            </div>
          </div>
          <div class="font-medium text-sm pt-2 text-gray-900">
            {{ observation?.text }}
          </div>
        </div>
      </div>
    </template>
  </div>
</template>
