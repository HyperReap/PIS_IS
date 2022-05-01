<template>
    <h2>Neuklizené pokoje</h2>
    <div class="mark-as-uncleaned">
        <select name="cleanedRooms" v-model="currentlySolvedUncleanedRoomId">
            <option value="" disabled selected>Číslo pokoje</option>
            <option v-for="cleanedRoom in cleanedRooms" :value="cleanedRoom.id">{{ cleanedRoom.roomNumber }}</option>
        </select>
        <el-button :class="[{ 'disable-pointer-events': !isTechnician }, 'markUncleanedButton']" type="primary" @click="markAsUncleaned()">Označit jako neuklizený</el-button>
    </div>
    <el-row v-if="uncleanedRoom">
        <el-col :span="24" class="uncleanedRooms">
            <div :class="[{ 'disable-pointer-events': !isCleaningLady }, 'uncleanedRoom']" v-for="uncleanedRoom in uncleanedRooms" :key="uncleanedRoom.id">
                <p><strong>Pokoj {{ uncleanedRoom.roomNumber }}</strong></p>
                <div class="markerWrap" v-if="isCleaningLady">
                    <el-button type="primary" @click="markAsCleaned(uncleanedRoom.id)">Uklizeno</el-button>
                </div>
            </div>
        </el-col>
    </el-row>
</template>
<script lang="js">
import { ElMessage } from 'element-plus'
export default {
  components: {
    ElMessage
  },
  data() {
    return {
      uncleanedRooms: [],
      cleanedRooms: [],
      currentlySolvedUncleanedRoomId: null,
      isCleaningLady: false,
      isTechnician: false,
    };
  },
  created() {
    this.loadUncleanedRooms();
    this.loadCleanedRooms()
    this.isCleaningLady = this.$store.getters.getLoggedUserRole == 4 ? true : false;
    this.isTechnician = this.$store.getters.getLoggedUserRole == 3 ? true : false;
  },
  
  methods: {
    loadUncleanedRooms() {
      this.uncleanedRoom = [];
      this.$root.loading = !this.$root.loading
      fetch('api/Room/GetAllUncleaned')
          .then(r => r.json())
          .then(json => {
            this.uncleanedRooms = json;
            this.$root.loading = !this.$root.loading
            ElMessage({"message": "Neuklizené pokoje načteny", "type": "success", "custom-class": "message-class"});
          })
          .catch(error => {
            this.$root.loading = !this.$root.loading
            ElMessage.error({
              "message": "Nepodařilo se načíst neuklizené pokoje!",
              "custom-class": "message-class",
              "grouping": true
            });
            console.log(error);
          });
    },
    loadCleanedRooms() {
      this.$root.loading = !this.$root.loading
      fetch('api/Room/GetAllCleaned')
          .then(r => r.json())
          .then(json => {
            this.cleanedRooms = json;
            this.$root.loading = !this.$root.loading
            ElMessage({"message": "Uklizené pokoje načteny", "type": "success", "custom-class": "message-class"});
          })
          .catch(error => {
            this.$root.loading = !this.$root.loading
            ElMessage.error({
              "message": "Nepodařilo se načíst Uklizené pokoje!",
              "custom-class": "message-class",
              "grouping": true
            });
            console.log(error);
          });
    },
    markAsCleaned(id) {
      if (this.isCleaningLady) {
        fetch('api/Room/MarkAsCleaned?roomId=' + id, {
          method: 'GET',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
        })
        .then(r => {
            if (r.status === 200) {
            ElMessage({"message": "Uklizeno!", "type": "success", "custom-class": "message-class"});
            this.uncleanedRooms = this.uncleanedRooms.filter(function (uncleanedRoom) {
                return uncleanedRoom.id !== id;
            });
            } else {
            ElMessage.error({
                "message": "Nepodařilo se označit jako uklizeno!",
                "custom-class": "message-class",
                "grouping": true
            });
            }
            return;
        })
        .catch(error => {
            ElMessage.error({
            "message": "Nepodařilo se označit jako uklizeno!",
            "custom-class": "message-class",
            "grouping": true
            });
            console.log(error);
        });
      }
    },
    markAsUncleaned() {
      if (this.isTechnician && this.currentlySolvedUncleanedRoomId !== null) {
        fetch('api/Room/MarkAsUncleaned?roomId=' + this.currentlySolvedUncleanedRoomId, {
          method: 'GET',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
        })
        .then(r => {
            if (r.status === 200) {
            ElMessage({ "message": "Označeno jako neuklizeno!", "type": "success", "custom-class": "message-class" });
            this.updateUncleanedList(this.currentlySolvedUncleanedRoomId);
            }
            else {
            ElMessage.error({ "message": "Nepodařilo se označit jako neuklizeno!", "custom-class": "message-class", "grouping": true });
            }
            return;
        })
        .catch(error => {
            ElMessage.error({ "message": "Nepodařilo se označit jako neuklizeno!", "custom-class": "message-class", "grouping": true });
            console.log(error);
        });
      }
      else {
          if (!this.isTechnician) {
              ElMessage.error({ "message": "Jedine technik může označit pokoj jako neuklizený!", "custom-class": "message-class", "grouping": true });
          }
          else {
              ElMessage.error({ "message": "Vyberte pokoj!", "custom-class": "message-class", "grouping": true });
          }
      }
    },
    updateUncleanedList(id) {
      if (this.isTechnician) {
        this.$root.loading = !this.$root.loading
        fetch('api/Room/Get?id=' + id, {
          method: 'GET',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
        })
        .then(r => r.json())
        .then(json => {
            this.uncleanedRooms.push(json);
            this.$root.loading = !this.$root.loading
            ElMessage({"message": "Neuklizený pokoj načten", "type": "success", "custom-class": "message-class"});
        })
        .catch(error => {
            this.$root.loading = !this.$root.loading
            ElMessage.error({
            "message": "Nepodařilo se načíst neuklizený pokoj!",
            "custom-class": "message-class",
            "grouping": true
            });
            console.log(error);
        });
      }
    }
  }
}
</script>
<style scoped>
h2{
    margin-bottom: 20px;
}
.mark-as-uncleaned {
  display: flex;
}
.markUncleanedButton{
    margin-left: 20px;
}
.mark-as-uncleaned .el-button {
  border-bottom-left-radius: 0;
  border-top-left-radius: 0;
}

.uncleanedRooms {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-start;
  margin-top: 20px;
}
.uncleanedRoom {
  width: 10%;
  flex-basis: 10%;
  margin-bottom: 20px;
  padding: 20px;
  border: 1px solid var(--el-border-color);
  margin-right: 5%;
}
.uncleanedRoom:nth-child(7n){
  margin-right: 0;
}
.uncleanedRoom p {
  text-overflow: ellipsis;
  text-align: center;
  overflow: hidden;
  width: 100%;
  white-space: nowrap;
}

.markerWrap {
  display: flex;
  justify-content: center;
}

.markerWrap .el-button {
  margin-top: 20px;
}
.uncleanedRoom .el-button {
  margin-left: auto;
  margin-right: auto;
  margin-top: 20px;
}

.uncleanedRoom:hover{
  border: 1px solid var(--el-color-primary);
  cursor: pointer;
}
.uncleanedRoom.disable-pointer-events{
  pointer-events: none;
}
.markUncleanedButton.disable-pointer-events{
  pointer-events: none;
}
strong{
  font-weight: 500;
  display: block;
}
.failure strong{
  margin-bottom: 20px;
}
h3{
  font-weight: 500;
  margin-bottom: 20px;
  font-size: 18px;
}
.mark-as-uncleaned select{
    min-width: 200px;
}
@media screen and (max-width: 1360px) {
    .uncleanedRoom {
        width: 15%;
        flex-basis: 15%;
        margin-right: 6.25%;
    }
    .uncleanedRoom:nth-child(7n) {
        margin-right: 6.25%;
    }
    .uncleanedRoom:nth-child(5n) {
        margin-right: 0;
    }
}
@media screen and (max-width: 992px) {
  .uncleanedRooms{
    justify-content: space-between
  }
  .uncleanedRoom {
    width: 45%;
    flex-basis: 45%;
    margin-right: 0;
  }
}
@media screen and (max-width: 630px) {
  .uncleanedRoom {
    width: 100%;
    flex-basis: 100%;
  }
}
@media screen and (max-width: 480px) {
    .mark-as-uncleaned {
        flex-direction: column;
    }
    .markUncleanedButton{
        margin-left: 0;
        margin-top: 20px;
    }
}
</style>